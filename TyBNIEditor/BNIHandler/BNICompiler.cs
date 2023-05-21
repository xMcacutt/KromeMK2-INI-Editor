using FastColoredTextBoxNS;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace TyBNIEditor
{
    internal class BNICompiler
    {
        public static byte[] OutputData;
        public static string[] InputData;
        public static List<string[]> Sections;
        public static List<string> FieldStrings;
        public static BNI BNI;
        public static int CurrentLineIndex;
        public static ushort RollingStringCount;
        public static List<ushort> SubSectionLineIndices;

        public static void Export(string[] data, string path)
        {
            BNI = new BNI();
            BNI.BNIPath = Regex.Replace(data[0], @"\p{C}+", "");
            InputData = data.Skip(2).ToArray();
            FieldStrings = new List<string>();
            GenerateSections();
            byte[] stringTableBytes = GenerateStringTable();
            byte[] shortTableBytes = GenerateShortTable();
            GenerateLines();
            byte[] lineBytes = CompileLines(BNI.Lines);
            BNI.DataLength = 0x44 + lineBytes.Length + shortTableBytes.Length + stringTableBytes.Length;
            BNI.StringTableOffset = lineBytes.Length + shortTableBytes.Length;
            BNI.ShortTableOffset = lineBytes.Length;
            BNI.LineCount = lineBytes.Length / 16;
            BNI.SectionCount = BNI.Sections.Count;
            byte[] headerBytes = CompileHeader();
            byte[] BNIbytes = CompileBNI(headerBytes, lineBytes, shortTableBytes, stringTableBytes);
            using (FileStream fs = File.Create(path))
            {
                fs.Write(BNIbytes, 0, BNIbytes.Length);
            }
        }

        public static void GenerateSections()
        {
            Sections = Utility.SplitLines(InputData);
            foreach (string[] sectionLines in Sections)
            {
                //CLEAN AND SET UP SECTION NAME
                Section section = new Section();
                section.Name = Regex.Replace(sectionLines[0], @"\p{C}+", "");
                BNI.StringHashSet.Add(section.Name);
                
                //ITERATE OVER LINES IN SECTION
                for(CurrentLineIndex = 1; CurrentLineIndex < sectionLines.Length; CurrentLineIndex++)
                {
                    //GET AND CLEAN LINE
                    string line = sectionLines[CurrentLineIndex];
                    int indentationLevel = 0;
                    string cleanedLine = Regex.Replace(line, @"\p{C}+", "");

                    //INITIALIZE DATA FOR FIELD WITH NO STRINGS (SUBSECTION
                    string fieldName;
                    string fieldText = string.Empty;
                    int firstSpaceIndex = cleanedLine.TrimEnd().IndexOf(' ');
                    fieldName = cleanedLine;

                    //SET STRING DATA FOR FIELD IF PRESENT
                    if (firstSpaceIndex != -1)
                    {
                        fieldName = cleanedLine.Substring(0, firstSpaceIndex);
                        fieldText = cleanedLine.Substring(firstSpaceIndex + 1);
                    }
                    string[] fieldStrings = cleanedLine.Replace(", ", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                    //DEAL WITH LONG HEADER STRINGS SEPARATELY
                    if (section.Name == "HEADER" && (fieldName == "h1" || fieldName == "h2" || fieldName == "h3"))
                    {
                        fieldStrings = fieldText.Split(new[] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    //DISCRIMINATE BETWEEN FIELD AND SUBSECTION (SUBSECTION MARKER HAS NO STRINGS)
                    int stringCount = fieldStrings.Length;
                    if(stringCount == 0 && CurrentLineIndex < sectionLines.Length - 1)
                    {
                        //SET SUBSECTION NAME
                        string subSectionName = cleanedLine;
                        int currentIndentationLevel;
                        List<string> subSectionLines = new List<string>();
                        CurrentLineIndex++;
                        line = sectionLines[CurrentLineIndex];
                        currentIndentationLevel = line.TakeWhile(char.IsWhiteSpace).Count() / 2;
                        while (indentationLevel < currentIndentationLevel && CurrentLineIndex < sectionLines.Length)
                        {
                            line = sectionLines[CurrentLineIndex];
                            cleanedLine = Regex.Replace(line, @"\p{C}+", "");
                            currentIndentationLevel = cleanedLine.TakeWhile(char.IsWhiteSpace).Count() / 2;
                            subSectionLines.Add(cleanedLine);
                            CurrentLineIndex++;
                            if (CurrentLineIndex < sectionLines.Length)
                            {
                                line = sectionLines[CurrentLineIndex];
                                cleanedLine = Regex.Replace(line, @"\p{C}+", "");
                                currentIndentationLevel = cleanedLine.TakeWhile(char.IsWhiteSpace).Count() / 2;
                            }
                        }
                        if(subSectionLines.Count == 0)
                        {
                            Field field = new Field();
                            field.Name = subSectionName;
                            BNI.StringHashSet.Add(fieldName);
                            section.Fields.Add(field);
                        }
                        else
                        {
                            SubSection subSection = GenerateSubSection(subSectionName, subSectionLines.ToArray(), indentationLevel);
                            section.SubSections.Add(subSection);
                            section.Fields.Add(subSection);
                        }
                    }
                    else
                    {
                        section.Fields.Add(GenerateField(fieldName, fieldStrings, stringCount));
                    }
                }
                BNI.Sections.Add(section);
            }
        }

        public static SubSection GenerateSubSection(string sectionName, string[] sectionLines, int baseIndentationLevel)
        {
            SubSection section = new SubSection();
            section.Name = sectionName;
            BNI.StringHashSet.Add(sectionName);
            int currentIndentationLevel;
            for(int i = 0; i < sectionLines.Length; i++)
            {
                string line = sectionLines[i];
                currentIndentationLevel = line.TakeWhile(char.IsWhiteSpace).Count() / 2;
                if(currentIndentationLevel == baseIndentationLevel + 1)
                {
                    //INITIALIZE DATA FOR FIELD WITH NO STRINGS (SUBSECTION)
                    string fieldName;
                    string fieldText = string.Empty;
                    int firstSpaceIndex = line.TrimEnd().TrimStart().IndexOf(' ');
                    fieldName = line;

                    //SET STRING DATA FOR FIELD IF PRESENT
                    if (firstSpaceIndex != -1)
                    {
                        fieldName = line.Substring(0, firstSpaceIndex);
                        fieldText = line.Substring(firstSpaceIndex + 1);
                    }
                    string[] fieldStrings = line.Replace(", ", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                    //DISCRIMINATE BETWEEN FIELD AND SUBSECTION (SUBSECTION MARKER HAS NO STRINGS)
                    if (fieldStrings.Length == 0 && i < sectionLines.Length - 1)
                    {
                        string subSectionName = line.TrimStart();
                        int currentIndentation;
                        List<string> subSectionLines = new List<string>();
                        i++;
                        line = sectionLines[i];
                        currentIndentation = line.TakeWhile(char.IsWhiteSpace).Count() / 2;
                        while (currentIndentationLevel < currentIndentation && i < sectionLines.Length)
                        {
                            line = sectionLines[i];
                            currentIndentation = line.TakeWhile(char.IsWhiteSpace).Count() / 2;
                            subSectionLines.Add(line);
                            i++;
                        }
                        if (subSectionLines.Count == 0)
                        {
                            Field field = new Field();
                            field.Name = subSectionName;
                            BNI.StringHashSet.Add(fieldName);
                            section.Fields.Add(field);
                        }
                        else
                        {
                            SubSection subSection = GenerateSubSection(subSectionName, subSectionLines.ToArray(), currentIndentationLevel);
                            section.SubSections.Add(subSection);
                            section.Fields.Add(subSection);
                        }
                    }
                    else
                    {
                        section.Fields.Add(GenerateField(fieldName, fieldStrings, fieldStrings.Length));
                    }
                }
            }
            CurrentLineIndex--;
            return section;
        }

        public static Field GenerateField(string fieldName, string[] fieldStrings, int stringCount)
        {
            Field field = new Field();
            field.Name = fieldName;
            BNI.StringHashSet.Add(fieldName);
            for (int i = 0; i < stringCount; i++)
            {
                FieldStrings.Add(fieldStrings[i]);
                field.Strings.Add(fieldStrings[i]);
                BNI.StringHashSet.Add(fieldStrings[i]);
            }
            return field;
        }

        public static byte[] GenerateStringTable()
        {
            BNI.StringDictionary = new Dictionary<string, ushort>();
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (string str in BNI.StringHashSet)
                {
                    BNI.StringDictionary.Add(str, (ushort)stream.Position);
                    stream.Write(Encoding.ASCII.GetBytes(str), 0, Encoding.ASCII.GetBytes(str).Length);
                    stream.WriteByte(0x0);
                    while (stream.Position % 4 != 0)
                    {
                        stream.WriteByte(0x0);
                    }
                }
                return stream.ToArray();
            }
        }

        public static byte[] GenerateShortTable()
        {
            using(MemoryStream stream = new MemoryStream())
            {
                foreach(string str in FieldStrings)
                {
                    if(BNI.StringDictionary.TryGetValue(str, out ushort position))
                    {
                        stream.Write(BitConverter.GetBytes(position / 4), 0, 2);
                    }
                }
                return stream.ToArray();
            }
        }

        public static void GenerateLines()
        {
            RollingStringCount = 0;
            Line spacer = new Line { FieldStringCount = 0xFFFF };
            foreach(Section section in BNI.Sections)
            {
                BNI.StringDictionary.TryGetValue(section.Name, out ushort stringOffset);
                Line sectionNameLine = new Line()
                {
                    FieldNameOffset = 0xFFFF,
                    MaskNameOffset = 0xFFFF,
                    SectionNameOffset = (ushort)(stringOffset / 4)
                };
                BNI.Lines.Add(sectionNameLine);
            }
            BNI.Lines.Add(spacer);
            int currentSection = 0;
            foreach(Section section in BNI.Sections)
            {
                SubSectionLineIndices = new List<ushort>();
                BNI.Lines[currentSection].DataStartLineIndex = (ushort)BNI.Lines.Count;
                currentSection++;
                foreach (Field field in section.Fields)
                {
                    BNI.Lines.Add(GenerateFieldLine(field));
                }
                BNI.Lines.Add(spacer);
                int currentSubSection = 0;
                foreach(SubSection subSection in section.SubSections)
                {
                    BNI.Lines[SubSectionLineIndices[currentSubSection]].DataStartLineIndex = (ushort)BNI.Lines.Count;
                    currentSubSection++;
                    foreach(Field field in subSection.Fields)
                    {
                        BNI.Lines.Add(GenerateFieldLine(field));
                    }
                    BNI.Lines.Add(spacer);
                }
            }
        } 

        public static Line GenerateFieldLine(Field field)
        {
            ushort stringOffset;
            BNI.StringDictionary.TryGetValue(field.Name, out stringOffset);
            Line fieldLine = new Line()
            {
                SectionNameOffset = 0xFFFF,
                MaskNameOffset = 0xFFFF,
                DataStartLineIndex = 0xFFFF,
                FieldStringCount = (ushort)field.Strings.Count,
                FieldNameOffset = (ushort)(stringOffset / 4),
                RollingFieldStringCount = RollingStringCount
            };
            RollingStringCount += (ushort)field.Strings.Count;
            if (field is SubSection subSection)
            {
                SubSectionLineIndices.Add((ushort)BNI.Lines.Count);
                BNI.StringDictionary.TryGetValue(subSection.Name, out stringOffset);
                fieldLine.FieldNameOffset = (ushort)(stringOffset / 4);
                fieldLine.SectionNameOffset = 0xFFFF;
            }
            return fieldLine;
        }

        public static byte[] CompileLines(List<Line> lines)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                byte[] padding = new byte[4];
                foreach(Line line in lines)
                {
                    stream.Write(BitConverter.GetBytes(line.FieldStringCount), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.SectionNameOffset), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.FieldNameOffset), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.RollingFieldStringCount), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.DataStartLineIndex), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.MaskNameOffset), 0, 2);
                    stream.Write(padding, 0, 4);
                }
                return stream.ToArray();
            }
        }
        
        public static byte[] CompileHeader()
        {
            using(MemoryStream stream = new MemoryStream())
            {
                byte[] path = Encoding.ASCII.GetBytes(BNI.BNIPath);
                byte[] pathPadding = new byte[0x20 - path.Length];
                path = path.Concat(pathPadding).ToArray();
                byte[] padding = Enumerable.Repeat((byte)0xFF, 0xC).ToArray();
                stream.Write(path, 0, 0x20);
                stream.Write(new byte[] { 0x64, 0x00, 0x00, 0x00, }, 0, 4);
                stream.Write(BitConverter.GetBytes(BNI.LineCount), 0, 4);
                stream.Write(BitConverter.GetBytes(BNI.DataLength), 0, 4);
                stream.Write(BitConverter.GetBytes(BNI.StringTableOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(BNI.ShortTableOffset), 0, 4);
                stream.Write(padding, 0, 0xC);
                stream.Write(BitConverter.GetBytes(BNI.SectionCount), 0, 4);
                return stream.ToArray();
            }
        }

        public static byte[] CompileBNI(byte[] header, byte[] lines, byte[] shortTable, byte[] stringTable)
        {
            return header.Concat(lines).Concat(shortTable).Concat(stringTable).ToArray();
        }


    }
}
