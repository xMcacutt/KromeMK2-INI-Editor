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
            GenerateFields();
            return GenerateOutput();
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
                if(sectionIndex == BNI.SectionCount - 1) section.LineCount = ((((BNI.ShortTable1Offset - 0x44) / 0x10) - 1) - line.DataStartLineIndex) - 1;
                else section.LineCount = (BNI.Lines[sectionIndex + 1].DataStartLineIndex - line.DataStartLineIndex) - 1;
                section.Name = Utility.ReadString(Data, BNI.StringTableOffset + (line.SectionNameOffset * 4));
                section.DataLines = BNI.Lines.GetRange(line.DataStartLineIndex, section.LineCount - 1);
                BNI.Sections.Add(section);
            }
        }

        public static void GenerateFields()
        {
            foreach(Section section in BNI.Sections)
            {
                foreach(Line line in section.DataLines)
                {
                    if (line.FieldStringCount == 0xFFFF) break;
                    Field field = new Field();
                    if (line.FieldStringCount == 0 && line.DataStartLineIndex != 0xFFFF)
                    {
                        field.IsSubSection = true;
                        field.SubSectionIndex = section.SubSections.Count;
                        field.Name = Utility.ReadString(Data, BNI.StringTableOffset + (line.FieldNameOffset * 4));
                        section.Fields.Add(field);
                        SubSection subSection = new SubSection();
                        subSection.Name = field.Name;
                        subSection.StartLineIndex = line.DataStartLineIndex;
                        subSection.Index = section.SubSections.Count;
                        int lineIndex = subSection.StartLineIndex;
                        while (BNI.Lines[lineIndex].FieldStringCount != 0xFFFF)
                        {
                            Field subField = new Field();
                            subField.Name = Utility.ReadString(Data, BNI.StringTableOffset + (BNI.Lines[lineIndex].FieldNameOffset * 4));
                            for (int stringIndex = 0; stringIndex < BNI.Lines[lineIndex].FieldStringCount; stringIndex++)
                            {
                                int shortTableOffset = (BNI.Lines[lineIndex].RollingFieldStringCount + stringIndex) * 2;
                                int stringTableOffset = BitConverter.ToInt16(Data, (BNI.ShortTable1Offset + shortTableOffset)) * 4;
                                subField.Strings.Add(Utility.ReadString(Data, BNI.StringTableOffset + stringTableOffset));
                            }
                            subSection.Fields.Add(subField);
                            lineIndex++;
                        }
                        section.SubSections.Add(subSection);
                        continue;
                    }
                    field.Name = Utility.ReadString(Data, BNI.StringTableOffset + (line.FieldNameOffset * 4));
                    for(int stringIndex = 0; stringIndex < line.FieldStringCount; stringIndex++)
                    {
                        int shortTableOffset = (line.RollingFieldStringCount + stringIndex) * 2;
                        int stringTableOffset = BitConverter.ToInt16(Data, (BNI.ShortTable1Offset + shortTableOffset)) * 4;
                        field.Strings.Add(Utility.ReadString(Data, BNI.StringTableOffset + stringTableOffset));
                    }
                    section.Fields.Add(field);
                }
            }
        }

        public static string[] GenerateOutput()
        {
            List<string> output = new List<string>();
            foreach(Section section in BNI.Sections)
            {
                output.Add(section.Name);
                foreach(Field field in section.Fields)
                {
                    if (field.IsSubSection)
                    {
                        SubSection subSection = section.SubSections.First(x => x.Index == field.SubSectionIndex);
                        output.Add(subSection.Name);
                        foreach (Field subField in subSection.Fields) output.Add("    " + subField.Name + " " + string.Join(", ", subField.Strings));
                        continue;
                    }
                    output.Add(field.Name + " " + string.Join(", ", field.Strings));
                }
                output.Add(" ");
            }
            return output.ToArray();
        }
    }
}
