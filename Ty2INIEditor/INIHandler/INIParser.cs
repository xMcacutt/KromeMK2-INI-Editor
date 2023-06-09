﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FastColoredTextBoxNS;
using static System.Net.Mime.MediaTypeNames;
using Ty2INIEditor.INIHandler;

namespace Ty2INIEditor
{
    internal class INIParser
    {
        public static byte[] Data;
        public static INI INI;
        public static int LineIndex;
        public static List<string> Text;
        public static List<int> LinesToSkip;
        public static int IndentationLevel;

        public static string[] Import(string path)
        {
            Data = File.ReadAllBytes(path);
            SettingsHandler.Settings.LittleEndian = Data[0x20] == 0x64;
            INI = new INI
            {
                Path = Utility.ReadString(Data, 0),
                LineCount = DataRead.ToInt32(Data, 0x24),
                DataLength = DataRead.ToInt32(Data, 0x28),
                StringTableOffset = 0x44 + DataRead.ToInt32(Data, 0x2C),
                ShortTableOffset = 0x44 + DataRead.ToInt32(Data, 0x30),
                SectionCount = DataRead.ToInt32(Data, 0x40)
            };
            ReadLines();
            GenerateSections();
            return Text.ToArray();
        }

        public static void ReadLines()
        {
            for (int i = 0; i < INI.LineCount; i++)
            {
                Line line = new Line();
                line.FieldStringCount = DataRead.ToUInt16(Data, 0x44 + (i * 0x10));
                line.SectionNameOffset = DataRead.ToUInt16(Data, 0x44 + (i * 0x10) + 0x2);
                line.FieldNameOffset = DataRead.ToUInt16(Data, 0x44 + (i * 0x10) + 0x4);
                line.RollingFieldStringCount = DataRead.ToUInt16(Data, 0x44 + (i * 0x10) + 0x6);
                line.DataStartLineIndex = DataRead.ToUInt16(Data, 0x44 + (i * 0x10) + 0x8);
                line.MaskNameOffset = DataRead.ToUInt16(Data, 0x44 + (i * 0x10) + 0xA);
                INI.Lines.Add(line);
            }
        }

        public static void GenerateSections()
        {
            Text = new List<string>() { INI.Path, "" };
            LinesToSkip = new List<int>();
            Stack<int> lineIndexStack = new Stack<int>();
            IndentationLevel = 0;
            for (LineIndex = 0; LineIndex < INI.LineCount; LineIndex++)
            {
                if (LinesToSkip.Contains(LineIndex)) continue;
                Line line = INI.Lines[LineIndex];
                if (line.Handled) continue;
                line.Handled = true;
                if (line.FieldStringCount == 0xFFFF)
                {
                    if(lineIndexStack.Count > 0) LineIndex = lineIndexStack.Pop();
                    continue;
                }

                string lineText = new string(' ', lineIndexStack.Count * 2);
                if (line.SectionNameOffset != 0xFFFF)
                {
                    Text.Add(lineText);
                    lineText += "name " + Utility.ReadString(Data, INI.StringTableOffset + (line.SectionNameOffset * 4)) + " ";
                }
                if (line.FieldNameOffset != 0xFFFF)
                {
                    lineText += Utility.ReadString(Data, INI.StringTableOffset + (line.FieldNameOffset * 4));
                    for (int i = 0; i < line.FieldStringCount; i++)
                    {
                        int stringTableOffset = DataRead.ToInt16(Data, INI.ShortTableOffset + (line.RollingFieldStringCount + i) * 2);
                        lineText += $" {Utility.ReadString(Data, INI.StringTableOffset + stringTableOffset * 4)}";
                    }
                    if (line.MaskNameOffset != 0xFFFF)
                    {
                        lineText += $@" \{Utility.ReadString(Data, INI.StringTableOffset + line.MaskNameOffset * 4)}\";
                    }
                }
                if (line.DataStartLineIndex != 0xFFFF)
                {
                    lineIndexStack.Push(LineIndex);
                    LineIndex = line.DataStartLineIndex - 1;
                }
                Text.Add(lineText);
            }
        }
    }
}
