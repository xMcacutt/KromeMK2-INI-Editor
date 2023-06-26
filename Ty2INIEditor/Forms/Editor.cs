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
using System.Collections.Generic;
using Ty2INIEditor.Properties;
using TradeWright.UI.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ty2INIEditor
{
    public partial class Editor : Form
    {
        public TextStyle KeywordsStyle;
        public TextStyle SectionNamesStyle;
        public TextStyle FieldNamesStyle;
        public TextStyle NumbersStyle;
        public TextStyle FieldTextStyle;
        public List<string> OpenFilePaths = new List<string>();
        AutocompleteMenu popupMenu;

        public Editor(string filePath)
        {
            Fonts.Setup();
            Themes.Load();
            SettingsHandler.Setup();

            InitializeComponent();
            InitializeProject(filePath);
            InitializeTab(filePath);
            FileTC.TabClosing += new EventHandler<TabControlCancelEventArgs>(FileTC_Closing);
            InitializeColors();
            InitializeFonts();
            Task.Run(() => ListenForFilePaths());
        }

        public static void InitializeProject(string filePath)
        {
            Project project;
            if (filePath.EndsWith(".mk2proj")) project = Project.LoadProjectFromFile(filePath);
            else
            {
                project = new Project()
                {
                    Name = "New Project " + DateTime.Now,
                    Description = "Description",
                    FileNames = new string[] { "New File" }
                };
            }
            if(Program.ProjectManager == null || Program.ProjectManager.IsDisposed) 
            { 
                Program.ProjectManager = new ProjectManager();
            }
            Program.ProjectManager.CurrentProject = project;
            Program.ProjectManager.LoadProject();

            Program.ProjectManager.Shown += (sender, e) =>
            {
                Point startLoc = Program.Editor.Location;
                startLoc.Offset(new Point(Program.Editor.Width - 5, 0));
                Program.ProjectManager.Location = startLoc;
            };
            Program.ProjectManager.Show();
        }

        public void InitializeTab(string filePath)
        {
            if(FileTC.Controls.Count == 1 && string.IsNullOrWhiteSpace(FileTC.SelectedTab.Controls[0].Text) && FileTC.SelectedTab.Text.Replace("*", "") == "New File")
            {
                FileTC.Controls.Remove(FileTC.Controls[0]);
            }
            if(filePath != "")
            {
                string[] allowedExtensions = { ".lv3", ".bni", ".model", ".mad", ".ini", ".ui", ".sound", ".txt" };
                if (!allowedExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Invalid File Extension", "Alert");
                    return;
                }
            }
            if (FileTC.TabPages.ContainsKey(filePath))
            {
                FileTC.SelectedTab = FileTC.TabPages[filePath];
                return;
            }
            TabPage tab = new TabPage();
            FastColoredTextBox FCTB = new FastColoredTextBox
            {
                AutoScrollMinSize = new Size(29, 18),
                BackBrush = null,
                BracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2,
                CharHeight = 18,
                CharWidth = 9,
                Cursor = Cursors.IBeam,
                Dock = DockStyle.Fill,
                IsReplaceMode = false,
                LeftBracket = '(',
                LeftBracket2 = '{',
                RightBracket = ')',
                RightBracket2 = '}',
                Location = new Point(3, 3),
                Name = "FCTB",
                Paddings = new Padding(0),
                Size = new Size(921, 470),
                TabIndex = 5,
                Zoom = 100,
                ForeColor = SettingsHandler.Colors.MainText,
                DisabledColor = SettingsHandler.Colors.BackgroundDark,
                SelectionColor = Color.FromArgb(60, 0, 0, 255),
                LineNumberColor = SettingsHandler.Colors.MainText,
                IndentBackColor = SettingsHandler.Colors.BackgroundSuperLight,
                BackColor = SettingsHandler.Colors.BackgroundLight,
                CaretColor = SettingsHandler.Colors.MainText,
                Font = Fonts.Standard
            };
            FCTB.TextChanged += new EventHandler<TextChangedEventArgs>(FCTB_TextChanged);
            FCTB.KeyDown += new KeyEventHandler(FCTB_KeyDown);

            popupMenu = new AutocompleteMenu(FCTB);
            popupMenu.MinFragmentLength = 2;
            popupMenu.Items.SetAutocompleteItems(Program.KnownStrings);
            popupMenu.Items.MaximumSize = new Size(300, 400);
            popupMenu.Items.Width = 300;
            popupMenu.Font = Fonts.Standard;
            popupMenu.BackColor = SettingsHandler.Colors.BackgroundSuperLight;
            popupMenu.ForeColor = SettingsHandler.Colors.MainText;
            popupMenu.HoveredColor = SettingsHandler.Colors.BackgroundLight;
            popupMenu.SelectedColor = SettingsHandler.Colors.BackgroundLight;

            tab.Text = "New File";
            tab.BorderStyle = BorderStyle.None;
            tab.Controls.Add(FCTB);
            FileTC.TabPages.Add(tab);
            FileTC.SelectedTab = tab;
            if (filePath != "")
            {
                tab.Name = filePath;
                if (filePath.EndsWith(".txt")) FCTB.Text = File.ReadAllText(filePath);
                else FCTB.Text = string.Join("\n", INIParser.Import(filePath));
                tab.Text = Path.GetFileName(filePath);
            }
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
                            Invoke((MethodInvoker)(() => InitializeTab(filePath)));
                        }
                    }
                }
            }
        }

        public void InitializeColors()
        {
            FileTC.DisplayStyleProvider.TextColorSelected = SettingsHandler.Colors.MainText;
            FileTC.DisplayStyleProvider.TextColorUnselected = SettingsHandler.Colors.MainText;
            FileTC.DisplayStyleProvider.TextColorFocused = SettingsHandler.Colors.MainText;
            FileTC.DisplayStyleProvider.TabColorSelected1 = SettingsHandler.Colors.BackgroundDark;
            FileTC.DisplayStyleProvider.TabColorSelected2 = SettingsHandler.Colors.BackgroundDark;
            FileTC.DisplayStyleProvider.TabColorFocused1 = SettingsHandler.Colors.BackgroundDark;
            FileTC.DisplayStyleProvider.TabColorFocused2 = SettingsHandler.Colors.BackgroundDark;
            FileTC.DisplayStyleProvider.TabColorUnSelected1 = SettingsHandler.Colors.BackgroundLight;
            FileTC.DisplayStyleProvider.TabColorUnSelected2 = SettingsHandler.Colors.BackgroundLight;
            foreach(TabPage tab in FileTC.Controls)
            {
                ((FastColoredTextBox)tab.Controls[0]).ForeColor = SettingsHandler.Colors.MainText;
                ((FastColoredTextBox)tab.Controls[0]).DisabledColor = SettingsHandler.Colors.BackgroundDark;
                ((FastColoredTextBox)tab.Controls[0]).SelectionColor = Color.FromArgb(60, 0, 0, 255);
                ((FastColoredTextBox)tab.Controls[0]).LineNumberColor = SettingsHandler.Colors.MainText;
                ((FastColoredTextBox)tab.Controls[0]).IndentBackColor = SettingsHandler.Colors.BackgroundSuperLight;
                ((FastColoredTextBox)tab.Controls[0]).BackColor = SettingsHandler.Colors.BackgroundLight;
                ((FastColoredTextBox)tab.Controls[0]).CaretColor = SettingsHandler.Colors.MainText;
            }
            Menu.ForeColor = SettingsHandler.Colors.MainText;
            Menu.BackColor = SettingsHandler.Colors.BackgroundLight;
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
        }

        private void InitializeFonts()
        {
            FileTC.Font = Fonts.SmallUI;
            Menu.Font = Fonts.SmallUI;
        }

        private void FileTC_Closing(object sender, TabControlCancelEventArgs e)
        {
            if (FileTC.SelectedTab.Text.EndsWith("*"))
            {
                DialogResult result = MessageBox.Show("This file has unsaved changes.\nClosing the tab will cause loss of data.", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Save the changes
                }
                else if (result == DialogResult.No)
                {
                    // Discard the changes
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if(FileTC.Controls.Count == 1)
            {
                InitializeTab("");
            }
        }


        private void FCTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!FileTC.SelectedTab.Text.EndsWith("*")) FileTC.SelectedTab.Text += "*";
            e.ChangedRange.ClearStyle(SectionNamesStyle);
            e.ChangedRange.ClearStyle(FieldNamesStyle);
            e.ChangedRange.ClearStyle(FieldTextStyle);
            e.ChangedRange.ClearStyle(KeywordsStyle);
            e.ChangedRange.ClearStyle(NumbersStyle);
            e.ChangedRange.SetStyle(NumbersStyle, @"(?<!\w)(-?\d+(\.\d+)?)(?!\w)");
            e.ChangedRange.SetStyle(KeywordsStyle, @"\b(none|true|false)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(SectionNamesStyle, @"name (.+)");
            if (Program.FieldNamesRegEx != null)
            {
                e.ChangedRange.SetStyle(FieldNamesStyle, Program.FieldNamesRegEx, RegexOptions.Multiline);
                e.ChangedRange.SetStyle(FieldTextStyle, Program.FieldNamesRegEx + @"(?!\s*$).*", RegexOptions.Multiline);
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
            if(Program.Preferences == null || Program.Preferences.IsDisposed) Program.Preferences = new Preferences();
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
            InitializeTab(path);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSelect = new SaveFileDialog
            {
                Filter = "Text Files (.txt)|*.txt",
                FileName = FileTC.SelectedTab.Controls[0].Text
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            if (!path.EndsWith(".txt"))
            {
                path += ".txt";
            }
            File.WriteAllText(path, FileTC.SelectedTab.Controls[0].Text);
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

            string filePath = INICompiler.Compile(FileTC.SelectedTab.Controls[0].Text.Split('\n'), path);

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
            if (FileTC.SelectedTab.Controls[0].Text.Split('\n').Length != 0) fileSelect.FileName = Path.GetFileName(Regex.Replace(FileTC.SelectedTab.Controls[0].Text.Split('\n')[0], @"\p{C}+", ""));
            if (!fileSelect.FileName.EndsWith(".bni")) fileSelect.FileName += ".bni";
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            INICompiler.Compile(FileTC.SelectedTab.Controls[0].Text.Split('\n'), path);
            MessageBox.Show("INI Generated", "Success");
        }

        private void batchAppendCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FileTC.SelectedTab.Controls[0].Text))
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
                text += "\n\n" + FileTC.SelectedTab.Controls[0].Text;
                string fileName = Path.GetFileName(file);
                if (!fileName.EndsWith(".bni")) fileName += ".bni";
                INICompiler.Compile(text.Split('\n'), Path.Combine(outputPath, fileName));
            }
            MessageBox.Show("Appended Text And Generated INIs", "Success");
        }
    }
}
