using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ty2INIEditor.Forms;
using Ty2INIEditor.INIHandler;

namespace Ty2INIEditor
{
    internal static class Program
    {
        public static Editor Editor;
        public static ProjectManager ProjectManager;
        public static ProjectSettings ProjectSettings;
        public static Preferences Preferences;
        public static string BaseDirectory;
        public static string FieldNamesRegEx;
        public static string[] KnownStrings;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            BaseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string filePath = "";
            using (Mutex mutex = new Mutex(true, "INIEditorMutex", out bool isNewInstance))
            {
                if (args.Length > 0 || Environment.GetCommandLineArgs().Length > 1)
                {
                    filePath = args.Length > 0 ? args[0] : Environment.GetCommandLineArgs()[1];
                }
                if (isNewInstance)
                {
                    string[] sectionNames = File.ReadAllLines(Path.Combine(BaseDirectory, "Data/sectionNames.txt"));
                    string[] fieldNames = File.ReadAllLines(Path.Combine(BaseDirectory, "Data/fieldNames.txt"));
                    FieldNamesRegEx += @"^\s*\b(" + string.Join("|", fieldNames.Select(fn => Regex.Escape(fn))) + @")\b";
                    KnownStrings = sectionNames.Concat(fieldNames).ToArray();
                    Application.Run(Editor = new Editor(filePath));
                }
                else
                {
                    using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "FileLoadingPipe", PipeDirection.Out))
                    {
                        pipeClient.Connect(5000); // Timeout after 5 seconds if connection cannot be established
                        using (StreamWriter writer = new StreamWriter(pipeClient))
                        {
                            writer.WriteLine(filePath);
                        }
                    }
                }
            }
        }

        public static void InitializeAllColors()
        {
            ProjectManager.InitializeColors();
            Preferences.InitializeColors();
            Editor.InitializeColors();
            ProjectSettings.InitializeColors();
        }
    }
}
