using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ty2INIEditor.INIHandler
{
    internal class INICompiler
    {
        public static Dictionary<string, int> StringPositions;
        public static List<byte> StringTable;
        public static List<ushort> ShortList;
        public static ushort RollingStringCount;
        public static int LineIndex;
        public static int IndentationLevel;
        public static Line Spacer = new Line() { FieldStringCount = 0xFFFF };
        public static INI INI;
        public static byte[] ShortTableBytes;
        public static byte[] HeaderBytes;
        public static byte[] LineBytes;

        public static string Compile(string[] data, string path)
        {
            INI = new INI();
            StringTable = new List<byte>();
            ShortList= new List<ushort>();
            RollingStringCount = 0;
            StringPositions = new Dictionary<string, int>();
            IndentationLevel = 0;
            LineIndex = 0;

            INI.Path = Regex.Replace(data[0], @"\p{C}+", "");

            if(path.EndsWith(".rkv"))
            {
                path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(INI.Path));
                if (!path.EndsWith(".bni")) path += ".bni";
            }

            data = data.Skip(1).Select(s => string.Concat(s.Where(c => !char.IsControl(c)))).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            INI.Lines.AddRange(GenerateLines(data));
            CompileShortTable();
            INI.SectionCount = INI.Lines.TakeWhile(l => l.Type == "Section").Count();
            INI.LineCount = INI.Lines.Count();
            INI.DataLength = INI.Lines.Count() * 0x10 + ShortTableBytes.Length + StringTable.ToArray().Length + 0x44;
            INI.ShortTableOffset = INI.Lines.Count() * 0x10;
            INI.StringTableOffset = INI.ShortTableOffset + ShortTableBytes.Length;
            CompileHeader();
            CompileLines();
            byte[] Data = HeaderBytes.Concat(LineBytes).Concat(ShortTableBytes).Concat(StringTable.ToArray()).ToArray();
            File.WriteAllBytes(path, Data);
            return path;
        }

        public static List<Line> GenerateLines(string[] data)
        {
            List<Line> lines = new List<Line>();

            //FIRST PASS
            for(int i = 0; i < data.Length; i++)
            {
                Line line = new Line();
                line.Text = data[i];
                line.MaskNameOffset = 0xFFFF;
                if (Utility.GetIndentationLevel(data[i]) == IndentationLevel)
                {
                    if (data[i].TrimStart().StartsWith("name ")) line.Type = "Section";
                    else line.Type = "Field";

                    if(i < data.Length - 1)
                    {
                        int currentIndentation = Utility.GetIndentationLevel(data[i]);
                        int nextIndentation = Utility.GetIndentationLevel(data[i + 1]);
                        if (currentIndentation < nextIndentation)
                        {
                            line.ChildData = data.Skip(i + 1).TakeWhile(s => Utility.GetIndentationLevel(s) >= nextIndentation).ToArray();
                        }
                    }
                    LineIndex++;
                    lines.Add(line);
                }
            }
            LineIndex++;
            lines.Add(Spacer);

            int lineCount = lines.Count;
            for(int i = 0; i < lineCount; i++)
            {
                Line line = lines[i];
                if (line.Type == "Section")
                {
                    //Section
                    line.FieldStringCount = 0;
                    line.RollingFieldStringCount = 0;
                    line.FieldNameOffset = 0xFFFF;

                    string sectionName = line.Text.Split(' ')[1];
                    AddStringTableEntry(sectionName);
                    StringPositions.TryGetValue(sectionName, out int sectionNameOffset);
                    line.SectionNameOffset = (ushort)(sectionNameOffset / 4);
                }
                if (line.Type == "Field")
                {
                    //FIELD
                    line.SectionNameOffset = 0xFFFF;

                    string fieldName = line.Text.TrimStart().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    AddStringTableEntry(fieldName);
                    string[] strings = line.Text.TrimStart().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
                    line.FieldStringCount = (ushort)strings.Length;
                    StringPositions.TryGetValue(fieldName, out int fieldNameOffset);
                    line.FieldNameOffset = (ushort)(fieldNameOffset / 4);

                    line.RollingFieldStringCount = RollingStringCount;
                    foreach (string s in strings)
                    {
                        RollingStringCount++;
                        AddStringTableEntry(s);
                        AddShortTableEntry(s);
                    }
                }
                if (line.ChildData == null)
                {
                    line.DataStartLineIndex = 0xFFFF;
                    continue;
                }
                line.DataStartLineIndex = (ushort)LineIndex;
                IndentationLevel++;
                lines.AddRange(GenerateLines(line.ChildData).ToArray());
                IndentationLevel--;
            }
            return lines;
        }

        public static void AddStringTableEntry(string text)
        {
            string entry = text.TrimStart();
            if (!StringPositions.ContainsKey(entry))
            {
                StringPositions.Add(entry, StringTable.Count);
                StringTable.AddRange(Encoding.ASCII.GetBytes(entry));
                StringTable.Add((byte)0);
                StringTable.AddRange(new byte[4 - StringTable.Count % 4]);
            }
        }

        public static void AddShortTableEntry(string entry)
        {
            StringPositions.TryGetValue(entry, out int stringOffset);
            ShortList.Add((ushort)(stringOffset / 4));
        }

        public static void CompileShortTable()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (ushort u in ShortList)
                {
                    stream.Write(BitConverter.GetBytes(u), 0, 2);
                }
                ShortTableBytes = stream.ToArray();
            }
        }

        public static void CompileHeader()
        {

            using (MemoryStream stream = new MemoryStream())
            {
                byte[] path = Encoding.ASCII.GetBytes(INI.Path);
                byte[] pathPadding = new byte[0x20 - path.Length];
                path = path.Concat(pathPadding).ToArray();
                byte[] padding = Enumerable.Repeat((byte)0xFF, 0xC).ToArray();
                stream.Write(path, 0, 0x20);
                stream.Write(new byte[] { 0x64, 0x00, 0x00, 0x00, }, 0, 4);
                stream.Write(BitConverter.GetBytes(INI.LineCount), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.DataLength), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.StringTableOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.ShortTableOffset), 0, 4);
                stream.Write(padding, 0, 0xC);
                stream.Write(BitConverter.GetBytes(INI.SectionCount), 0, 4);
                HeaderBytes = stream.ToArray();
            }
        }

        public static void CompileLines()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] padding = new byte[4];
                foreach (Line line in INI.Lines)
                {
                    stream.Write(BitConverter.GetBytes(line.FieldStringCount), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.SectionNameOffset), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.FieldNameOffset), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.RollingFieldStringCount), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.DataStartLineIndex), 0, 2);
                    stream.Write(BitConverter.GetBytes(line.MaskNameOffset), 0, 2);
                    stream.Write(padding, 0, 4);
                }
                LineBytes = stream.ToArray();
            }
        }
    }
}
