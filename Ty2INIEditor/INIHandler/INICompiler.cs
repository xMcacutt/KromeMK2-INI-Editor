using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ty2INIEditor.INIHandler
{
    internal class INICompiler
    {
        public static Dictionary<string, int> StringPositions;
        public static List<byte> StringTable;
        public static List<ushort> ShortList;
        public static List<(string, int)> SectionNames;
        public static ushort RollingStringCount;
        public static int LineIndex;
        public static int IndentationLevel;
        public static Line Spacer = new Line() { FieldStringCount = 0xFFFF };
        public static INI INI;
        public static byte[] ShortTableBytes;
        public static byte[] HeaderBytes;
        public static byte[] LineBytes;
        public static byte[] HashTableBytes;
        public static byte[] BinarySearchTableBytes;

        public static string Compile(string[] data, string path)
        {
            INI = new INI();
            StringTable = new List<byte>();
            ShortList = new List<ushort>();
            SectionNames = new List<(string, int)>();
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

            data = data.Skip(1).Select(s => string.Concat(s.Where(c => !char.IsControl(c)))).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            INI.Lines.AddRange(GenerateLines(data));
            CompileShortTable();
            INI.SectionCount = INI.Lines.Where(l => l.Type == "Section").Count();
            CompileHashTable();
            CompileBinarySearchTable();
            INI.LineCount = INI.Lines.Count();
            INI.DataLength = INI.Lines.Count() * 0x10 + ShortTableBytes.Length + StringTable.ToArray().Length + HashTableBytes.Length + BinarySearchTableBytes.Length;
            INI.ShortTableOffset = INI.Lines.Count() * 0x10;
            INI.StringTableOffset = INI.ShortTableOffset + ShortTableBytes.Length;
            INI.HashTableOffset = INI.StringTableOffset + StringTable.ToArray().Length;
            INI.BinarySearchTableOffset = INI.HashTableOffset + HashTableBytes.Length;
            CompileHeader();
            CompileLines();
            byte[] Data = HeaderBytes.Concat(LineBytes).Concat(ShortTableBytes).Concat(StringTable.ToArray()).Concat(HashTableBytes).Concat(BinarySearchTableBytes).ToArray();
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
                line.FieldStringCount = 0;
                line.RollingFieldStringCount = 0;
                line.FieldNameOffset = 0xFFFF;
                line.SectionNameOffset = 0xFFFF;
                line.MaskNameOffset = 0xFFFF;
                if (Utility.GetIndentationLevel(data[i]) == IndentationLevel)
                {
                    if (data[i].TrimStart().StartsWith("name "))
                    {
                        line.SectionNameOffset = 0xFFFE;
                        if (data[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length > 2) line.FieldNameOffset = 0xFFFE;
                    }
                    else line.FieldNameOffset = 0xFFFE;

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
                if (line.SectionNameOffset == 0xFFFE)
                {
                    line.Type = "Section";
                    string sectionName = line.Text.Split(' ')[1].Replace(@"___", " ");
                    AddStringTableEntry(sectionName);
                    StringPositions.TryGetValue(sectionName, out int sectionNameOffset);
                    line.SectionNameOffset = (ushort)(sectionNameOffset / 4);
                    SectionNames.Add((sectionName, i));
                    line.Text = string.Join(" ", line.Text.Split(' ').Skip(2).ToArray());
                }
                if (line.FieldNameOffset == 0xFFFE)
                {
                    //FIELD
                    string fieldName = line.Text.TrimStart().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("___", " ");
                    AddStringTableEntry(fieldName);
                    string[] strings;
                    strings = line.Text.TrimStart().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
                    if (fieldName == "text") strings = new[] { new string(line.Text.TrimEnd().TrimStart().Skip(5).ToArray()) };
                    if (strings.Length > 0 && strings[strings.Length - 1].StartsWith(@"\"))
                    {
                        string mask = strings[strings.Length - 1];
                        strings = strings.Take(strings.Length - 1).ToArray();
                        mask = mask.Replace(@"\", "");
                        AddStringTableEntry(mask);
                        StringPositions.TryGetValue(mask, out int maskNameOffset);
                        line.MaskNameOffset = (ushort)(maskNameOffset / 4);
                    }
                    line.FieldStringCount = (ushort)strings.Length;
                    StringPositions.TryGetValue(fieldName, out int fieldNameOffset);
                    line.FieldNameOffset = (ushort)(fieldNameOffset / 4);

                    line.RollingFieldStringCount = RollingStringCount;
                    foreach (string str in strings)
                    {
                        string s = str.Replace("___", " ");
                        RollingStringCount++;
                        AddStringTableEntry(s);
                        AddShortTableEntry(s);
                    }
                    if (strings.Length == 0) line.RollingFieldStringCount = 0;
                }
                if (line.ChildData == null)
                {
                    if(line.FieldStringCount != 0xFFFF) line.DataStartLineIndex = 0xFFFF;
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
            string entry = text.TrimEnd().TrimStart();
            if (!StringPositions.ContainsKey(entry))
            {
                StringPositions.Add(entry, StringTable.Count);
                StringTable.AddRange(Encoding.ASCII.GetBytes(entry));
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
                stream.Write(path, 0, 0x20);
                stream.Write(new byte[] { 0x64, 0x00, 0x00, 0x00, }, 0, 4);
                stream.Write(BitConverter.GetBytes(INI.LineCount), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.DataLength), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.StringTableOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.ShortTableOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.HashTableOffset), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.HashDivisor), 0, 4);
                stream.Write(BitConverter.GetBytes(INI.BinarySearchTableOffset), 0, 4);
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

        public static void CompileHashTable()
        {
            INI.HashDivisor = (int)Math.Round(INI.SectionCount * 1.33f, 0);
            if(SectionNames.Count == 0)
            {
                HashTableBytes = new byte[] { 0x0, 0x0 };
                return;
            }
            SortedDictionary<uint, (string, int)> map = new SortedDictionary<uint, (string, int)>();
            foreach (var entry in SectionNames)
            {
                uint hash = Utility.CalculateHash(entry.Item1, (uint)INI.HashDivisor);
                if (hash > INI.HashDivisor) hash = 0;
                while (map.Keys.Contains(hash))
                {
                    hash++;
                    if (hash > INI.HashDivisor - 1) hash = 0;
                }
                map.Add(hash, entry);
            }
            using (MemoryStream stream = new MemoryStream())
            {
                for (uint i = 0; i < map.Last().Key + 1; i++)
                {
                    if (map.TryGetValue(i, out (string, int) entry)) stream.Write(BitConverter.GetBytes(entry.Item2), 0, 2);
                    else stream.Write(new byte[] {0xFF, 0xFF}, 0, 2);
                }
                HashTableBytes = stream.ToArray();
            }
        }

        public static void CompileBinarySearchTable()
        {
            if (SectionNames.Count == 0)
            {
                BinarySearchTableBytes = new byte[] { 0x0, 0x0 };
                return;
            }
            SectionNames.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (var entry in SectionNames)
                {
                    stream.Write(BitConverter.GetBytes(entry.Item2), 0, 2);
                }
                BinarySearchTableBytes = stream.ToArray();
            }
        }
    }
}
