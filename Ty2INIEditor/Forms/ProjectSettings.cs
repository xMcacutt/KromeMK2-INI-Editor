using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ty2INIEditor.Forms
{
    public partial class ProjectSettings : Form
    {
        public ProjectSettings()
        {
            InitializeComponent();
            InitializeColors();
            InitializeFonts();
        }

        public void InitializeColors()
        {
            BackColor = SettingsHandler.Colors.BackgroundDark;
            WindowLayoutTable.BackColor = SettingsHandler.Colors.BackgroundLight;
            ProjectName.ForeColor = SettingsHandler.Colors.MainText;
            ProjectDesc.ForeColor = SettingsHandler.Colors.MainText;
            ProjectNameField.BackColor = SettingsHandler.Colors.BackgroundDark;
            ProjectDescField.BackColor = SettingsHandler.Colors.BackgroundDark;
            ProjectNameField.ForeColor = SettingsHandler.Colors.MainText;
            ProjectDescField.ForeColor = SettingsHandler.Colors.MainText;
            ConfirmButton.ForeColor = SettingsHandler.Colors.MainText;
            ConfirmButton.BackColor = SettingsHandler.Colors.BackgroundDark;
        }

        public void InitializeFonts()
        {
            ProjectName.Font = Fonts.LargeUI;
            ProjectDesc.Font = Fonts.LargeUI;
            ProjectNameField.Font = Fonts.Standard;
            ProjectDescField.Font = Fonts.Standard;
            ConfirmButton.Font = Fonts.MediumUI;
        }

        public void Initialize(Project project, bool edit)
        {
            ConfirmButton.Text = edit ? "Apply" : "Create";
            if(project != null)
            {
                ProjectNameField.Text = project.Name;
                ProjectDescField.Text = project.Description;
            }
        }

        private void ProjectSettings_Load(object sender, EventArgs e)
        {
            ConfirmButton.Left = (ConfirmButton.Parent.Width - ConfirmButton.Width) / 2;
        }

        private void ProjectDescField_TextChanged(object sender, EventArgs e)
        {
            ConfirmButton.Visible = !string.IsNullOrWhiteSpace(ProjectDescField.Text) && !string.IsNullOrWhiteSpace(ProjectNameField.Text);
        }

        private void ProjectNameField_TextChanged(object sender, EventArgs e)
        {
            ConfirmButton.Visible = !string.IsNullOrWhiteSpace(ProjectDescField.Text) && !string.IsNullOrWhiteSpace(ProjectNameField.Text);
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (Directory.GetDirectories(Path.Combine(Program.BaseDirectory, "Projects")).Contains(ProjectNameField.Text))
            {
                MessageBox.Show($"A project with name {ProjectNameField.Text} already exists.\nPlease choose another name or delete the project folder.", "Alert", MessageBoxButtons.OK);
                return;
            }    

            if(ConfirmButton.Text == "Create")
            {
                Program.ProjectManager.CurrentProject = Project.CreateProject(ProjectNameField.Text, ProjectDescField.Text);
                Program.ProjectManager.CurrentProject.GenerateProjectFile();
                Program.ProjectManager.LoadProject();
            }
            else
            {
                File.Delete(Program.ProjectManager.CurrentProject.mk2projPath);
                Directory.Move(Program.ProjectManager.CurrentProject.DirectoryPath, Path.Combine(Program.BaseDirectory, "Projects", ProjectNameField.Text));
                Program.ProjectManager.CurrentProject.Name = ProjectNameField.Text;
                Program.ProjectManager.CurrentProject.Description = ProjectDescField.Text;
                Program.ProjectManager.CurrentProject.DirectoryPath = Path.Combine(Program.BaseDirectory, "Projects", ProjectNameField.Text);
                Program.ProjectManager.CurrentProject.mk2projPath = Path.Combine(Program.ProjectManager.CurrentProject.DirectoryPath, ProjectNameField.Text + ".mk2proj");
                Program.ProjectManager.CurrentProject.GenerateProjectFile();
                Program.ProjectManager.UpdateDetails();
            }
            Program.ProjectSettings.Close();
        }
    }
}
