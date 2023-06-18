using FastColoredTextBoxNS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
            return Encoding.ASCII.GetString(bytes, position, endOfString - position);
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

    }
}
