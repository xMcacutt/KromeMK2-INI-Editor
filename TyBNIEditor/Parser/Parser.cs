using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TyBNIEditor
{
    internal class Parser
    {
        public const int LINE_LENGTH = 0x10;
        
        public static string[] Import(string path)
        {
            byte[] Data = File.ReadAllBytes(path);
            List<Section> entries = new List<Section>();

            BNI bni = new BNI
            {
                lv3Path = Utility.ReadString(Data, 0),
                LineCount = BitConverter.ToInt32(Data, 0x24),
                DataLength = BitConverter.ToInt32(Data, 0x28),
                StringTableOffset = 0x44 + BitConverter.ToInt32(Data, 0x2C),
                ShortTable1Offset = 0x44 + BitConverter.ToInt32(Data, 0x30),
                ShortTable2Offset = 0x44 + BitConverter.ToInt32(Data, 0x34),
                SectionCount = BitConverter.ToInt32(Data, 0x40)
            };
            for (int i = 0; i < bni.LineCount; i++)
            {
                Line line = new Line()
                {
                    StringCount = BitConverter.ToInt16(Data, 0x44 + (i * LINE_LENGTH)),
                    SectionNameOffset = BitConverter.ToUInt16(Data, 0x44 + (i * LINE_LENGTH) + 0x2),
                    FieldNameOffset1 = BitConverter.ToUInt16(Data, 0x44 + (i * LINE_LENGTH) + 0x4),
                    Index = BitConverter.ToUInt16(Data, 0x44 + (i * LINE_LENGTH) + 0x6),
                    FieldNameOffset2 = BitConverter.ToUInt16(Data, 0x44 + (i * LINE_LENGTH) + 0xA),
                };
                if (line.StringCount != -1)
                {
                    if (line.SectionNameOffset != 0xFFFF)
                    {
                        string name = Utility.ReadString(Data, bni.StringTableOffset + (line.SectionNameOffset * 4));
                        entries.Add(new Section(name));
                        line.SectionName = name;
                        continue;
                    }
                    if (line.FieldNameOffset1 != 0xFFFF)
                    {
                        string fieldName = Utility.ReadString(Data, bni.StringTableOffset + (line.FieldNameOffset1 * 4));
                        line.FieldName = fieldName;
                    }
                    if (line.FieldNameOffset2 != 0xFFFF)
                    {
                        string fieldName = Utility.ReadString(Data, bni.StringTableOffset + (line.FieldNameOffset2 * 4));
                        line.FieldName = fieldName;
                    }
                    for (int s = 0; s < line.StringCount; s++)
                    {
                        int idx = (line.Index + s) * 2;
                        int tableOffset = BitConverter.ToInt16(Data, (bni.ShortTable1Offset + idx)) * 4;
                        line.Strings.Add(Utility.ReadString(Data, bni.StringTableOffset + tableOffset));
                    }
                }
                bni.Lines.Add(line);
            }
            List<string> output = new List<string>();
            output.Add(bni.lv3Path);
            int x = 0;
            foreach(Line line in bni.Lines)
            {
                if (line.StringCount == -1 && x < bni.SectionCount)
                {
                    if(output.Count != 0) output.Add("");
                    output.Add(entries[x].Name);
                    x++;
                }
                string field = " ";
                if (!string.IsNullOrWhiteSpace(line.FieldName)) field = line.FieldName + ' ';
                field += string.Join(", ", line.Strings);
                if (!string.IsNullOrWhiteSpace(field)) output.Add(field);
            }
            return output.ToArray();
        }
    }
}
