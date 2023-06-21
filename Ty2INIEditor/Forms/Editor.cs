using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;
using Ty2INIEditor.Forms;
using Ty2INIEditor.INIHandler;
using System.Xml.Linq;
using WK.Libraries.BetterFolderBrowserNS;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace Ty2INIEditor
{
    public partial class Editor : Form
    {
        string _fieldNamesRegexExp;
        public TextStyle KeywordsStyle;
        public TextStyle SectionNamesStyle;
        public TextStyle FieldNamesStyle;
        public TextStyle NumbersStyle;
        public TextStyle FieldTextStyle;
        AutocompleteMenu popupMenu;

        public Editor(string filePath)
        {
            Fonts.Setup();
            Themes.Load();
            SettingsHandler.Setup();
            InitializeComponent();
            popupMenu = new AutocompleteMenu(FCTB);
            InitializeColors();
            InitializeFonts();
            string baseDirectory = Program.BaseDirectory;
            string sectionNamesFilePath = Path.Combine(baseDirectory, "Data/sectionNames.txt");
            string fieldNamesFilePath = Path.Combine(baseDirectory, "Data/fieldNames.txt");

            string[] sectionNames = File.ReadAllLines(sectionNamesFilePath);
            string[] fieldNames = File.ReadAllLines(fieldNamesFilePath);
            _fieldNamesRegexExp += @"^\s*\b(" + string.Join("|", fieldNames.Select(fn => Regex.Escape(fn))) + @")\b";

            popupMenu.MinFragmentLength = 2;
            popupMenu.Items.SetAutocompleteItems(sectionNames.Concat(fieldNames).ToArray());
            popupMenu.Items.MaximumSize = new Size(300, 400);
            popupMenu.Items.Width = 300;

            if (filePath != "") OpenFile(filePath);

            Task.Run(() => ListenForFilePaths());
        }

        private void ListenForFilePaths()
        {
            while (true)
            {
                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("FileLoadingPipe", PipeDirection.In))
                {

                    pipeServer.WaitForConnection();
                    using (StreamReader reader = new StreamReader(pipeServer))
                    {
                        string filePath = reader.ReadLine();
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            OpenFile(filePath);
                        }
                    }
                }
            }
        }

        public void InitializeColors()
        {
            popupMenu.BackColor = SettingsHandler.Colors.BackgroundSuperLight;
            popupMenu.ForeColor = SettingsHandler.Colors.MainText;
            popupMenu.HoveredColor = SettingsHandler.Colors.BackgroundLight;
            popupMenu.SelectedColor = SettingsHandler.Colors.BackgroundLight;
            FCTB.ForeColor = SettingsHandler.Colors.MainText;
            FCTB.LineNumberColor = SettingsHandler.Colors.MainText;
            FCTB.IndentBackColor = SettingsHandler.Colors.BackgroundSuperLight;
            FCTB.BackColor = SettingsHandler.Colors.BackgroundLight;
            FCTB.CaretColor = SettingsHandler.Colors.MainText;
            Menu.ForeColor = SettingsHandler.Colors.MainText;
            Menu.BackColor = SettingsHandler.Colors.BackgroundLight;
            FileNameLabel.ForeColor = SettingsHandler.Colors.MainText;
            ForeColor = SettingsHandler.Colors.MainText;
            BackColor = SettingsHandler.Colors.BackgroundDark;
            Brush keywordsBrush = new SolidBrush(SettingsHandler.Colors.Keywords);
            Brush sectionNamesBrush = new SolidBrush(SettingsHandler.Colors.SectionNames);
            Brush fieldNamesBrush = new SolidBrush(SettingsHandler.Colors.FieldNames);
            Brush fieldTextBrush = new SolidBrush(SettingsHandler.Colors.FieldText);
            Brush numbersBrush = new SolidBrush(SettingsHandler.Colors.Numbers);
            if (KeywordsStyle == null)
            {
                KeywordsStyle = new TextStyle(keywordsBrush, null, FontStyle.Regular);
                SectionNamesStyle = new TextStyle(sectionNamesBrush, null, FontStyle.Regular);
                FieldNamesStyle = new TextStyle(fieldNamesBrush, null, FontStyle.Regular);
                FieldTextStyle = new TextStyle(fieldTextBrush, null, FontStyle.Regular);
                NumbersStyle = new TextStyle(numbersBrush, null, FontStyle.Regular);
            }
            else
            {
                KeywordsStyle.ForeBrush = keywordsBrush;
                SectionNamesStyle.ForeBrush = sectionNamesBrush;
                FieldNamesStyle.ForeBrush = fieldNamesBrush;
                FieldTextStyle.ForeBrush = fieldTextBrush;
                NumbersStyle.ForeBrush = numbersBrush;
            }
            string text = FCTB.Text;
            FCTB.Text = "";
            FCTB.Text = text;
        }

        private void InitializeFonts()
        {
            FCTB.Font = Fonts.Standard;
            popupMenu.Font = Fonts.Standard;
            FileNameLabel.Font = Fonts.SmallUI;
            Menu.Font = Fonts.SmallUI;
        }

        private void FCTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(SectionNamesStyle);
            e.ChangedRange.ClearStyle(FieldNamesStyle);
            e.ChangedRange.ClearStyle(FieldTextStyle);
            e.ChangedRange.ClearStyle(KeywordsStyle);
            e.ChangedRange.ClearStyle(NumbersStyle);
            e.ChangedRange.SetStyle(NumbersStyle, @"(?<!\w)(-?\d+(\.\d+)?)(?!\w)");
            e.ChangedRange.SetStyle(KeywordsStyle, @"\b(none|true|false)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(SectionNamesStyle, @"name (.+)");
            if (_fieldNamesRegexExp != null)
            {
                e.ChangedRange.SetStyle(FieldNamesStyle, _fieldNamesRegexExp, RegexOptions.Multiline);
                e.ChangedRange.SetStyle(FieldTextStyle, _fieldNamesRegexExp + @"(?!\s*$).*", RegexOptions.Multiline);
            }
        }

        private void FCTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Control && popupMenu != null)
            {
                popupMenu.Show();
                e.Handled = true;
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Program.Preferences == null) Program.Preferences = new Preferences();
            Program.Preferences.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileSelect = new OpenFileDialog
            {
                Filter = "Config Files (*.ini, *.model, *.mad, *.lv3)|*.*|Text Files (*.txt)|*.txt"
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            FileNameLabel.Text = Path.GetFileName(path);
            if (path.EndsWith(".txt"))
            {
                FCTB.Text = File.ReadAllText(path);
                return;
            }
            FCTB.Text = string.Join("\n", INIParser.Import(path));
        }

        public void OpenFile(string path)
        {
            string[] allowedExtensions = { ".lv3", ".bni", ".model", ".mad", ".ini", ".ui", ".sound", ".txt" };
            if (!allowedExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Invalid File Extension", "Alert");
                return;
            }
            FileNameLabel.Text = Path.GetFileName(path);
            if (path.EndsWith(".txt"))
            {
                FCTB.Text = File.ReadAllText(path);
                return;
            }
            FCTB.Text = string.Join("\n", INIParser.Import(path));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSelect = new SaveFileDialog
            {
                Filter = "Text Files (.txt)|*.txt",
                FileName = FileNameLabel.Text
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            if (!path.EndsWith(".txt"))
            {
                path += ".txt";
            }
            File.WriteAllText(path, FCTB.Text);
            MessageBox.Show("Text Saved", "Success");
        }

        private void asTestRKVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSelect = new SaveFileDialog
            {
                Filter = "rkv Files (.rkv)|*.rkv",
                FileName = "Patch_PC.rkv"
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;

            string filePath = INICompiler.Compile(FCTB.Text.Split('\n'), path);

            RKV2_Tools.RKV rkv = new RKV2_Tools.RKV();
            rkv.Repack(filePath, path);
            File.Delete(filePath);
            MessageBox.Show("RKV Generated", "Success");
        }

        private void asINIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSelect = new SaveFileDialog
            {
                Filter = "ini File (.*)|*.*",
            };
            if (FCTB.Text.Split('\n').Length != 0) fileSelect.FileName = Path.GetFileName(Regex.Replace(FCTB.Text.Split('\n')[0], @"\p{C}+", ""));
            if (!fileSelect.FileName.EndsWith(".bni")) fileSelect.FileName += ".bni";
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            INICompiler.Compile(FCTB.Text.Split('\n'), path);
            MessageBox.Show("INI Generated", "Success");
        }

        private void batchAppendCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FCTB.Text))
            {
                MessageBox.Show("No Text To Append", "Alert");
                return;
            }
            
            BetterFolderBrowser fbd = new BetterFolderBrowser();
            fbd.Title = "Select Input Folder";
            DialogResult result = fbd.ShowDialog();
            if (result != DialogResult.OK) return;
            string[] allowedExtensions = { ".lv3", ".bni", ".model", ".mad", ".ini", ".ui", ".sound" };
            string inputPath = fbd.SelectedPath;
            string[] files = Directory.GetFiles(inputPath);
            // Check if any file doesn't have an allowed extension using LINQ
            bool badFiles = files.Any(file => !allowedExtensions.Contains(Path.GetExtension(file)));
            if (badFiles)
            {
                MessageBox.Show("Invalid File Extensions In Input Directory", "Alert");
                return;
            }
            fbd.Title = "Select Output Folder";
            result = fbd.ShowDialog();
            if (result != DialogResult.OK) return;
            string outputPath = fbd.SelectedPath;
            foreach(string file in files)
            {
                string text = string.Join("\n", INIParser.Import(file));
                text += "\n\n" + FCTB.Text;
                string fileName = Path.GetFileName(file);
                if (!fileName.EndsWith(".bni")) fileName += ".bni";
                INICompiler.Compile(text.Split('\n'), Path.Combine(outputPath, fileName));
            }
            MessageBox.Show("Appended Text And Generated INIs", "Success");
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCTB.Clear();
            FileNameLabel.Text = "No File Open";
        }
    }
}
