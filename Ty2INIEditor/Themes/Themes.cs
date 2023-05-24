using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ty2INIEditor
{
    internal class Themes
    {
        public static List<string> ThemeNames = new List<string>();

        public static void Load()
        {
            foreach(string s in Directory.GetFiles("./Themes"))
            {
                if (s.EndsWith(".json") && Path.GetFileNameWithoutExtension(s) != "Colors")
                {
                    ThemeNames.Add(Path.GetFileNameWithoutExtension(s));
                }
            }
        }
    }
}
