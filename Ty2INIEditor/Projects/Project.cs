using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ty2INIEditor
{
    public class Project
    {
        public string DirectoryPath;
        public string mk2projPath;
        public string Name;
        public string Description;
        public string[] FileNames;

        public void GenerateProjectFile()
        {
            string projectsDirectory = Path.Combine(Program.BaseDirectory, "Projects");
            if (!Directory.Exists(projectsDirectory)) Directory.CreateDirectory(projectsDirectory);
            DirectoryPath = Path.Combine(projectsDirectory, Name);
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
            mk2projPath = Path.Combine(DirectoryPath, Name + ".mk2proj");
            using (StreamWriter stream = File.CreateText(mk2projPath))
            {
                stream.WriteLine(Name + "\n");
                stream.WriteLine(Description + "\n");
                foreach(string fileName in FileNames) stream.Write(fileName + "\n");
            }
        }

        public static Project LoadProjectFromFile(string projectPath)
        {
            if(!Directory.Exists(Path.Combine(Program.BaseDirectory, "Projects", Path.GetFileNameWithoutExtension(projectPath))))
            {
                MessageBox.Show("A project with this name does not exist.\nI'm not sure how you're seeing this tbh.", "What?");
                return null;
            }
            Project project = new Project();
            string[] lines = File.ReadAllLines(projectPath);
            lines = lines.Where(str => !string.IsNullOrEmpty(str) && !str.StartsWith("#")).ToArray();
            project.mk2projPath = projectPath;
            project.Name = lines[0];
            project.DirectoryPath = Path.Combine(Program.BaseDirectory, "Projects", project.Name);
            project.Description = lines[1];
            project.FileNames = lines.Skip(2).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return project; 
        }
    }
}
