using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ty2INIEditor.Properties;
using System.IO;

namespace Ty2INIEditor
{
    internal class SettingsHandler
    {
        public static Colors Colors { get; set; }
        public static void Setup()
        {
            string json = File.ReadAllText("./Themes/Colors.json");
            Colors = JsonConvert.DeserializeObject<Colors>(json);
            Colors.Setup();
        }

        public static void Save(string name)
        {
            string json = JsonConvert.SerializeObject(Colors, Formatting.Indented);
            File.WriteAllText($"./Themes/{name}.json", json);
        }

        public static void Accept()
        {
            string json = JsonConvert.SerializeObject(Colors, Formatting.Indented);
            File.WriteAllText("./Themes/Colors.json", json);
        }

        public static void Load(string name)
        {
            string json = File.ReadAllText($"./Themes/{name}.json");
            Colors = JsonConvert.DeserializeObject<Colors>(json);
            Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
        }
    }
}
