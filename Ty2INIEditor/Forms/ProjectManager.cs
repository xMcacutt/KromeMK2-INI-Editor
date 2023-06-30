using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ty2INIEditor.Controls;
using WK.Libraries.BetterFolderBrowserNS;

namespace Ty2INIEditor.Forms
{
    public partial class ProjectManager : Form
    {
        private Project _currentProject;

        public Project CurrentProject
        {
            get
            {
                return _currentProject;
            }
            set
            {
                _currentProject = value;
                editProjectToolStripMenuItem.Enabled = _currentProject != null;
                addToolStripMenuItem.Enabled = _currentProject != null;
                sortToolStripMenuItem.Enabled = _currentProject != null;
                Program.Editor.SetMenuItemsEnabled(_currentProject != null);
            }
        }
        bool _dropDownOpen;
        Dictionary<string, string> _iconMappings;

        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);


        public ProjectManager()
        {
            InitializeComponent();
            ProjectMenuBar.Renderer = new CustomToolStripRenderer();
            InitializeColors();
            InitializeFonts();
            if (_iconMappings != null) return;
            _iconMappings =  Directory.EnumerateFiles(Path.Combine(Program.BaseDirectory, "Icons"), "*.ico").ToDictionary(
                iconFile => Path.GetFileNameWithoutExtension(iconFile).TrimStart('.'),
                iconFile => Path.GetFileName(iconFile)
            );
        }

        public void InitializeColors()
        {
            BackColor = SettingsHandler.Colors.BackgroundDark;
            ProjectFilesTable.BackColor = SettingsHandler.Colors.BackgroundLight;
            ProjectFilesTable.ForeColor = SettingsHandler.Colors.MainText;
            ProjectDetailsTable.BackColor = SettingsHandler.Colors.BackgroundLight;
            ProjectDetailsTable.ForeColor = SettingsHandler.Colors.MainText;
            ProjectName.BackColor = SettingsHandler.Colors.BackgroundLight;
            ProjectName.ForeColor = SettingsHandler.Colors.MainText;
            ProjectDescription.BackColor = SettingsHandler.Colors.BackgroundLight;
            ProjectDescription.ForeColor = SettingsHandler.Colors.MainText;
            ProjectMenuBar.ForeColor = SettingsHandler.Colors.MainText;
            ProjectMenuBar.BackColor = SettingsHandler.Colors.BackgroundLight;
            foreach(Control c in ProjectFilesTable.Controls)
            {
                if(c is Label) c.ForeColor = SettingsHandler.Colors.MainText;
            }
        }

        private void InitializeFonts()
        {
            ProjectName.Font = Fonts.MediumUI;
            ProjectDescription.Font = Fonts.SmallUI;
            ProjectFilesTable.Font = Fonts.Standard;
            ProjectMenuBar.Font = Fonts.SmallUI;
        }

        public void LoadProject()
        {
            ProjectName.Text = CurrentProject.Name;
            ProjectDescription.Text = CurrentProject.Description;
            foreach(string file in CurrentProject.FileNames)
            {
                InitializeFile(file);
            }
        }

        public void InitializeFile(string file)
        {
            if (file == "New File") return;
            Label fileNameLabel = new Label
            {
                Text = file,
                ForeColor = SettingsHandler.Colors.MainText,
                Font = Fonts.SmallUI,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 0, 3, 0)
            };
            fileNameLabel.DoubleClick += fileName_DoubleClick;
            fileNameLabel.Name = file;
            ProjectFilesTable.Controls.Add(fileNameLabel, 1, ProjectFilesTable.RowCount - 1);
            string extension = Path.GetFileName(file).Split('.')[1];
            string iconFileName = _iconMappings.ContainsKey(extension) ? _iconMappings[extension] : "unk.ico";
            Image image = new Icon(Path.Combine(Program.BaseDirectory, "Icons", iconFileName), new Size(16, 16)).ToBitmap();
            PictureBox pictureBox = new PictureBox
            {
                Image = image,
                Size = new Size(16, 16),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            ProjectFilesTable.Controls.Add(pictureBox, 0, ProjectFilesTable.RowCount - 1);
            ProjectFilesTable.RowCount += 1;
            ProjectFilesTable.AutoScroll = ProjectFilesTable.RowCount > 13;
            ProjectFilesTable.HorizontalScroll.Visible = false;
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Editor.FileTC.TabPages.Count != 1 || Program.Editor.FileTC.SelectedTab.Text != "New File")
            {
                var mbResult = MessageBox.Show("Opening a project will cause all open tabs to close.\nAre you sure you want to open a project?", "Alert", MessageBoxButtons.YesNo);
                if (mbResult == DialogResult.No) return;
                else Program.Editor.CloseTabs();
            }
            OpenFileDialog fileSelect = new OpenFileDialog
            {
                Filter = "MK2Proj files (.mk2proj)|*.mk2proj"
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            Editor.InitializeProject(path);
        }

        private void ProjectDescription_TextChanged(object sender, EventArgs e)
        {
            // amount of padding to add
            const int padding = 3;
            // get number of lines (first line is 0, so add 1)
            int numLines = ProjectDescription.GetLineFromCharIndex(ProjectDescription.TextLength) + 1;
            // get border thickness
            int border = ProjectDescription.Height - ProjectDescription.ClientSize.Height;
            // set height (height of one line * number of lines + spacing)
            ProjectDescription.Height = ProjectDescription.Font.Height * numLines + padding + border;
        }

        private void fileName_DoubleClick(object sender, EventArgs e)
        {
            Label fileName = sender as Label;
            string filePath = Path.Combine(CurrentProject.DirectoryPath, fileName.Text);
            Console.WriteLine(filePath);
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Sorry, that file does not exist.\nIt was likely removed without its mk2proj entry being deleted.\nThe entry will now be removed.", "Alert");
                List<string> lines = File.ReadAllLines(CurrentProject.mk2projPath).ToList();
                lines.RemoveAll(line => line == fileName.Text);
                File.WriteAllLines(CurrentProject.mk2projPath, lines);
                ProjectFilesTable.RemoveRow(ProjectFilesTable.GetRow(fileName));
                return;
            }
            Program.Editor.InitializeTab(filePath);
        }

        private void ProjectDescription_Enter(object sender, EventArgs e)
        { 
            ActiveControl = null;
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileSelect = new OpenFileDialog
            {
                Filter = "All Files|*.*"
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            string name = Path.GetFileName(path);
            if (Directory.GetFiles(CurrentProject.DirectoryPath).Contains(name))
            {
                MessageBox.Show("This file already exists in the project", "Alert");
                return;
            }
            File.Copy(path, Path.Combine(CurrentProject.DirectoryPath, name));
            InitializeFile(name);
            CurrentProject.AddFile(name);
        }

        public void ClearFilesTable()
        {
            ProjectFilesTable.Controls.Clear();
            ProjectFilesTable.RowCount = 1;
        }

        public void LoadFilesTable()
        {
            foreach (string file in CurrentProject.FileNames)
            {
                InitializeFile(file);
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Program.Editor.FileTC.TabPages.Count != 1 || Program.Editor.FileTC.SelectedTab.Text != "New File")
            {
                var mbResult = MessageBox.Show("Creating a new project will cause all open tabs to close.\nAre you sure you want to create a project?", "Alert", MessageBoxButtons.YesNo);
                if (mbResult == DialogResult.No) return;
                else Program.Editor.CloseTabs();
            }
            Program.Editor.CloseTabs();
            if (Program.ProjectSettings == null || Program.ProjectSettings.IsDisposed) Program.ProjectSettings = new ProjectSettings();
            Program.ProjectSettings.Initialize(null, false); 
            Program.ProjectSettings.Show();
        }

        private void editProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Editor.FileTC.TabPages.Count != 1 || Program.Editor.FileTC.SelectedTab.Text != "New File")
            {
                var mbResult = MessageBox.Show("Editing the current project will cause all open tabs to close.\nAre you sure you want to edit the project?", "Alert", MessageBoxButtons.YesNo);
                if (mbResult == DialogResult.No) return;
                else Program.Editor.CloseTabs();
            }
            if (Program.ProjectSettings == null || Program.ProjectSettings.IsDisposed) Program.ProjectSettings = new ProjectSettings();
            Program.ProjectSettings.Initialize(CurrentProject, true);
            Program.ProjectSettings.Show();
        }

        private void byNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentProject.SortFiles(SortingType.Name);
            ClearFilesTable();
            LoadFilesTable();
        }

        private void byFileTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentProject.SortFiles(SortingType.Extension);
            ClearFilesTable();
            LoadFilesTable();
        }

        internal void UpdateDetails()
        {
            ProjectName.Text = CurrentProject.Name;
            ProjectDescription.Text = CurrentProject.Description;
        }

        private void ProjectFilesTable_DragDrop(object sender, DragEventArgs e)
        {
            if (CurrentProject == null) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string path in files)
                {
                    string name = Path.GetFileName(path);
                    if (Directory.GetFiles(CurrentProject.DirectoryPath).Contains(name))
                    {
                        MessageBox.Show("This file already exists in the project", "Alert");
                        continue;
                    }
                    File.Copy(path, Path.Combine(CurrentProject.DirectoryPath, name));
                    InitializeFile(name);
                    CurrentProject.AddFile(name);
                }
            }
        }

        private void ProjectFilesTable_DragEnter(object sender, DragEventArgs e)
        {
            if(CurrentProject != null) e.Effect = DragDropEffects.Copy;
        }

        private void ProjectManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.ProjectManager.Hide();
            e.Cancel = true;
        }

        private void exportToRKVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BetterFolderBrowser fbd = new BetterFolderBrowser();
            fbd.Title = "Select Output Folder";
            DialogResult result = fbd.ShowDialog();
            if (result != DialogResult.OK) return;
            RKV rkv = new RKV();
            rkv.Repack(CurrentProject.DirectoryPath, fbd.SelectedPath);
            MessageBox.Show("RKV Generated", "Success");
        }
    }
}
