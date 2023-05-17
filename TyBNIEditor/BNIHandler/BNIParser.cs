using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FastColoredTextBoxNS;
using System.Xml.Linq;

namespace TyBNIEditor
{
    internal class BNIParser
    {
        public static byte[] Data;
        public static BNI BNI;

        public static string[] Import(string path)
        {
            Data = File.ReadAllBytes(path);
            BNI = new BNI
            {
                BNIPath = Utility.ReadString(Data, 0),
                LineCount = BitConverter.ToInt32(Data, 0x24),
                DataLength = BitConverter.ToInt32(Data, 0x28),
                StringTableOffset = 0x44 + BitConverter.ToInt32(Data, 0x2C),
                ShortTable1Offset = 0x44 + BitConverter.ToInt32(Data, 0x30),
                ShortTable2Offset = 0x44 + BitConverter.ToInt32(Data, 0x34),
                SectionCount = BitConverter.ToInt32(Data, 0x40)
            };
            ReadLines();
            GenerateSections();
            return GenerateOutput().ToArray();
        }

        public static void ReadLines()
        {
            for (int i = 0; i < BNI.LineCount; i++)
            {
                Line line = new Line()
                {
                    FieldStringCount = BitConverter.ToUInt16(Data, 0x44 + (i * 0x10)),
                    SectionNameOffset = BitConverter.ToUInt16(Data, 0x44 + (i * 0x10) + 0x2),
                    FieldNameOffset = BitConverter.ToUInt16(Data, 0x44 + (i * 0x10) + 0x4),
                    RollingFieldStringCount = BitConverter.ToUInt16(Data, 0x44 + (i * 0x10) + 0x6),
                    DataStartLineIndex = BitConverter.ToUInt16(Data, 0x44 + (i * 0x10) + 0x8),
                };
                BNI.Lines.Add(line);
            }
        }

        public static void GenerateSections()
        {
            Line line;
            for (int sectionIndex = 0; sectionIndex < BNI.SectionCount; sectionIndex++)
            {
                line = BNI.Lines[sectionIndex];
                Section section = new Section();
                section.Name = Utility.ReadString(Data, BNI.StringTableOffset + (line.SectionNameOffset * 4));
                section.Fields = GenerateFields(line.DataStartLineIndex);
                BNI.Sections.Add(section);
            }
        }
        public static List<Field> GenerateFields(int startLineIndex)
        {
            int lineIndex = startLineIndex;
            List<Field> fields = new List<Field>();
            while (BNI.Lines[lineIndex].FieldStringCount != 0xFFFF)
            {
                Line line = BNI.Lines[lineIndex];
                lineIndex++;
                if (line.FieldStringCount == 0 && line.DataStartLineIndex != 0xFFFF)
                {
                    string name = Utility.ReadString(Data, BNI.StringTableOffset + (line.FieldNameOffset * 4));
                    fields.Add(GenerateSubSection(name, line.DataStartLineIndex));
                    continue;
                }
                Field field = new Field
                {
                    Name = Utility.ReadString(Data, BNI.StringTableOffset + (line.FieldNameOffset * 4)),
                    Strings = ReadFieldStrings(line)
                };
                fields.Add(field);
            }
            return fields;
        }

        public static SubSection GenerateSubSection(string name, int startLineIndex)
        {
            SubSection subSection = new SubSection();
            subSection.Name = name;
            subSection.Fields = GenerateFields(startLineIndex);
            return subSection;
        }

        private static List<string> ReadFieldStrings(Line line)
        {
            List<string> strings = new List<string>();

            for (int stringIndex = 0; stringIndex < line.FieldStringCount; stringIndex++)
            {
                int shortTableOffset = (line.RollingFieldStringCount + stringIndex) * 2;
                int stringTableOffset = BitConverter.ToInt16(Data, (BNI.ShortTable1Offset + shortTableOffset)) * 4;
                strings.Add(Utility.ReadString(Data, BNI.StringTableOffset + stringTableOffset));
            }

            return strings;
        }

        public static List<string> GenerateOutput(List<Field> fields, int level)
        {
            List<string> output = new List<string>();
            foreach (Field field in fields)
            {
                output.AddRange(GenerateFieldOutput(field, level));
            }
            return output;
        }

        public static List<string> GenerateFieldOutput(Field field, int level)
        {
            List<string> output = new List<string>();
            string indent = new string(' ', level * 2);
            if (field is SubSection subSection)
            {
                output.Add(indent + field.Name);
                output.AddRange(GenerateOutput(subSection.Fields, level + 1));
            }
            else
            {
                output.Add(indent + field.Name + " " + string.Join(", ", field.Strings));
            }
            return output;
        }

        public static List<string> GenerateOutput()
        {
            List<string> output = new List<string>();
            foreach (Section section in BNI.Sections)
            {
                output.Add(section.Name);
                output.AddRange(GenerateOutput(section.Fields, 0));
                output.Add("");
            }
            return output;
        }
    }
}
