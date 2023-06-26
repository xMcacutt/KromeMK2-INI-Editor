using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ty2INIEditor.Forms
{
    public partial class ProjectManager : Form
    {
        public Project CurrentProject;
        bool _dropDownOpen;
        Dictionary<string, string> _iconMappings;

        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);


        public ProjectManager()
        {
            InitializeComponent();
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
                if (file == "New File") continue;
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
                string extension = Path.GetExtension(file).TrimStart('.');
                string iconFileName = _iconMappings.ContainsKey(extension) ? _iconMappings[extension] : "unk.ico";
                Image image = new Icon(Path.Combine(Program.BaseDirectory, "Icons", iconFileName), new Size(16,16)).ToBitmap();
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
        }


        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Sorry, that file does not exist.\nIt was likely removed without its mk2proj entry being deleted.\nThe entry will now be removed.", "Alert");
                List<string> lines = File.ReadAllLines(CurrentProject.mk2projPath).ToList();
                lines.RemoveAll(line => line == fileName.Text);
                File.WriteAllLines(CurrentProject.mk2projPath, lines);
                int rowIndex = ProjectFilesTable.GetRow(fileName);
                ProjectFilesTable.RowStyles.RemoveAt(rowIndex);

                return;
            }
            Program.Editor.InitializeTab(filePath);
        }

        private void ProjectDescription_Enter(object sender, EventArgs e)
        { 
            ActiveControl = null;
        }

        private void projectToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ProjectMenuBar.ForeColor = SettingsHandler.Colors.BackgroundDark;
        }

        private void projectToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            ProjectMenuBar.ForeColor = SettingsHandler.Colors.MainText;
            _dropDownOpen = false;
        }

        private void projectToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if(!_dropDownOpen) ProjectMenuBar.ForeColor = SettingsHandler.Colors.MainText;
        }

        private void projectToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            _dropDownOpen = true;
        }
    }
}
