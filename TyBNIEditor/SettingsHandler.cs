using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyBNIEditor.Properties;
using System.IO;

namespace TyBNIEditor
{
    internal class SettingsHandler
    {
        public static Colors Colors { get; set; }
        public static void Setup()
        {
            //MAIN SETTINGS
            string json = File.ReadAllText("./Colors.json");
            Colors = JsonConvert.DeserializeObject<Colors>(json);
            Colors.Setup();
        }
    }
}
