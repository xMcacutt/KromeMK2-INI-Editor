namespace Ty2INIEditor.Forms
{
    partial class ProjectManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectManager));
            this.WindowLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.ProjectFilesTable = new System.Windows.Forms.TableLayoutPanel();
            this.ProjectDetailsTable = new System.Windows.Forms.TableLayoutPanel();
            this.ProjectName = new System.Windows.Forms.Label();
            this.ProjectDescription = new System.Windows.Forms.RichTextBox();
            this.ProjectMenuBar = new System.Windows.Forms.MenuStrip();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byFileTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowLayoutTable.SuspendLayout();
            this.ProjectDetailsTable.SuspendLayout();
            this.ProjectMenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // WindowLayoutTable
            // 
            this.WindowLayoutTable.AutoSize = true;
            this.WindowLayoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowLayoutTable.ColumnCount = 1;
            this.WindowLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.WindowLayoutTable.Controls.Add(this.ProjectFilesTable, 0, 1);
            this.WindowLayoutTable.Controls.Add(this.ProjectDetailsTable, 0, 0);
            this.WindowLayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowLayoutTable.Location = new System.Drawing.Point(0, 24);
            this.WindowLayoutTable.Margin = new System.Windows.Forms.Padding(5);
            this.WindowLayoutTable.Name = "WindowLayoutTable";
            this.WindowLayoutTable.RowCount = 2;
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.Size = new System.Drawing.Size(406, 235);
            this.WindowLayoutTable.TabIndex = 0;
            // 
            // ProjectFilesTable
            // 
            this.ProjectFilesTable.AutoSize = true;
            this.ProjectFilesTable.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.ProjectFilesTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ProjectFilesTable.ColumnCount = 2;
            this.ProjectFilesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ProjectFilesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ProjectFilesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectFilesTable.Location = new System.Drawing.Point(5, 66);
            this.ProjectFilesTable.Margin = new System.Windows.Forms.Padding(5);
            this.ProjectFilesTable.MaximumSize = new System.Drawing.Size(0, 300);
            this.ProjectFilesTable.Name = "ProjectFilesTable";
            this.ProjectFilesTable.RowCount = 1;
            this.ProjectFilesTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ProjectFilesTable.Size = new System.Drawing.Size(396, 164);
            this.ProjectFilesTable.TabIndex = 0;
            // 
            // ProjectDetailsTable
            // 
            this.ProjectDetailsTable.AutoSize = true;
            this.ProjectDetailsTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ProjectDetailsTable.ColumnCount = 1;
            this.ProjectDetailsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ProjectDetailsTable.Controls.Add(this.ProjectName, 0, 0);
            this.ProjectDetailsTable.Controls.Add(this.ProjectDescription, 0, 1);
            this.ProjectDetailsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectDetailsTable.Location = new System.Drawing.Point(5, 5);
            this.ProjectDetailsTable.Margin = new System.Windows.Forms.Padding(5);
            this.ProjectDetailsTable.Name = "ProjectDetailsTable";
            this.ProjectDetailsTable.RowCount = 2;
            this.ProjectDetailsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ProjectDetailsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ProjectDetailsTable.Size = new System.Drawing.Size(396, 51);
            this.ProjectDetailsTable.TabIndex = 0;
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSize = true;
            this.ProjectName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProjectName.Location = new System.Drawing.Point(5, 5);
            this.ProjectName.Margin = new System.Windows.Forms.Padding(5);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(68, 13);
            this.ProjectName.TabIndex = 0;
            this.ProjectName.Text = "ProjectName";
            // 
            // ProjectDescription
            // 
            this.ProjectDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProjectDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.ProjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectDescription.Location = new System.Drawing.Point(5, 28);
            this.ProjectDescription.Margin = new System.Windows.Forms.Padding(5);
            this.ProjectDescription.MaxLength = 1000;
            this.ProjectDescription.Name = "ProjectDescription";
            this.ProjectDescription.ReadOnly = true;
            this.ProjectDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ProjectDescription.Size = new System.Drawing.Size(386, 18);
            this.ProjectDescription.TabIndex = 1;
            this.ProjectDescription.TabStop = false;
            this.ProjectDescription.Text = "ProjectDescription";
            this.ProjectDescription.TextChanged += new System.EventHandler(this.ProjectDescription_TextChanged);
            this.ProjectDescription.Enter += new System.EventHandler(this.ProjectDescription_Enter);
            // 
            // ProjectMenuBar
            // 
            this.ProjectMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.filesToolStripMenuItem});
            this.ProjectMenuBar.Location = new System.Drawing.Point(0, 0);
            this.ProjectMenuBar.Name = "ProjectMenuBar";
            this.ProjectMenuBar.Size = new System.Drawing.Size(406, 24);
            this.ProjectMenuBar.TabIndex = 1;
            this.ProjectMenuBar.Text = "menuStrip1";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.editProjectToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            this.projectToolStripMenuItem.DropDownClosed += new System.EventHandler(this.projectToolStripMenuItem_DropDownClosed);
            this.projectToolStripMenuItem.DropDownOpened += new System.EventHandler(this.projectToolStripMenuItem_DropDownOpened);
            this.projectToolStripMenuItem.MouseEnter += new System.EventHandler(this.projectToolStripMenuItem_MouseEnter);
            this.projectToolStripMenuItem.MouseLeave += new System.EventHandler(this.projectToolStripMenuItem_MouseLeave);
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveProjectToolStripMenuItem.Text = "Add File To Project";
            // 
            // editProjectToolStripMenuItem
            // 
            this.editProjectToolStripMenuItem.Name = "editProjectToolStripMenuItem";
            this.editProjectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.editProjectToolStripMenuItem.Text = "Edit Project";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.sortToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byNameToolStripMenuItem,
            this.byFileTypeToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.sortToolStripMenuItem.Text = "Sort..";
            // 
            // byNameToolStripMenuItem
            // 
            this.byNameToolStripMenuItem.Name = "byNameToolStripMenuItem";
            this.byNameToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.byNameToolStripMenuItem.Text = "By Name";
            // 
            // byFileTypeToolStripMenuItem
            // 
            this.byFileTypeToolStripMenuItem.Name = "byFileTypeToolStripMenuItem";
            this.byFileTypeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.byFileTypeToolStripMenuItem.Text = "By File Type";
            // 
            // ProjectManager
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(406, 259);
            this.Controls.Add(this.WindowLayoutTable);
            this.Controls.Add(this.ProjectMenuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ProjectMenuBar;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectManager";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " Project Manager";
            this.WindowLayoutTable.ResumeLayout(false);
            this.WindowLayoutTable.PerformLayout();
            this.ProjectDetailsTable.ResumeLayout(false);
            this.ProjectDetailsTable.PerformLayout();
            this.ProjectMenuBar.ResumeLayout(false);
            this.ProjectMenuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel WindowLayoutTable;
        private System.Windows.Forms.TableLayoutPanel ProjectFilesTable;
        private System.Windows.Forms.TableLayoutPanel ProjectDetailsTable;
        private System.Windows.Forms.Label ProjectName;
        private System.Windows.Forms.MenuStrip ProjectMenuBar;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.RichTextBox ProjectDescription;
        private System.Windows.Forms.ToolStripMenuItem editProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byFileTypeToolStripMenuItem;
    }
}