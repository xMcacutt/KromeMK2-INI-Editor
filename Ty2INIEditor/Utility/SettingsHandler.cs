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
        public static Settings Settings { get; set; }

        public static void Setup()
        {
            string colors = File.ReadAllText(Path.Combine(Program.BaseDirectory, "Themes/Colors.json"));
            Settings = new Settings();
            Colors = JsonConvert.DeserializeObject<Colors>(colors);
            Colors.Setup();
        }

        public static void Save(string name)
        {
            string colors = JsonConvert.SerializeObject(Colors, Formatting.Indented);
            File.WriteAllText(Path.Combine(Program.BaseDirectory, $"Themes/{name}.json"), colors);
        }

        public static void Accept()
        {
            string json = JsonConvert.SerializeObject(Colors, Formatting.Indented);
            File.WriteAllText(Path.Combine(Program.BaseDirectory, "Themes/Colors.json"), json);
        }

        public static void Load(string name)
        {
            string json = File.ReadAllText(Path.Combine(Program.BaseDirectory, $"Themes/{name}.json"));
            Colors = JsonConvert.DeserializeObject<Colors>(json);
            Colors.Setup();
            Program.InitializeAllColors();
        }
    }
}
