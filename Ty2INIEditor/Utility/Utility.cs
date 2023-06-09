﻿using FastColoredTextBoxNS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ty2INIEditor
{
    public static class Utility
    {
        public static string ReadString(byte[] bytes, int position)
        {
            int endOfString = Array.IndexOf<byte>(bytes, 0x0, position);
            if (endOfString == position) return string.Empty;
            string s =  Encoding.ASCII.GetString(bytes, position, endOfString - position);
            return s.Replace(" ", @"___");
        }

        public static uint CalculateHash(string str, uint div)
        {
            uint hash = 0;
            uint len = (uint)str.Length;
            for (int i = 0; i < len; i++)
            {
                hash = (uint)((hash * 0x11) + (str[i] | 0x20));
            }
            return hash % div;
        }

        public static int GetIndentationLevel(string text)
        {
            return (text.Length - text.TrimStart().Length) / 2;
        }

        public static List<string[]> SplitLines(string[] lines)
        {
            List<string[]> splitArrays = new List<string[]>();
            List<string> currentArray = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line) || Regex.IsMatch(line, @"^[\x00-\x1F\x7F]+$"))
                {
                    splitArrays.Add(currentArray.ToArray());
                    currentArray.Clear();
                }
                else
                {
                    currentArray.Add(line);
                }
            }
            return splitArrays;
        }

        public static string GetRelativePath(string inputDir, string file)
        {
            var inputDirParts = inputDir.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            var fileParts = file.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);

            int commonIndex = inputDirParts.Zip(fileParts, (a, b) => a.Equals(b, StringComparison.OrdinalIgnoreCase)).TakeWhile(x => x).Count();

            var relativePath = string.Join(Path.DirectorySeparatorChar.ToString(), Enumerable.Repeat("..", inputDirParts.Length - commonIndex).Concat(fileParts.Skip(commonIndex)));

            return relativePath;
        }
    }
}
