namespace Ty2INIEditor.Forms
{
    partial class ProjectSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSettings));
            this.WindowLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.ProjectNameField = new System.Windows.Forms.RichTextBox();
            this.ProjectDescField = new System.Windows.Forms.RichTextBox();
            this.ProjectName = new System.Windows.Forms.Label();
            this.ProjectDesc = new System.Windows.Forms.Label();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.WindowLayoutTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // WindowLayoutTable
            // 
            this.WindowLayoutTable.AutoSize = true;
            this.WindowLayoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowLayoutTable.ColumnCount = 1;
            this.WindowLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.WindowLayoutTable.Controls.Add(this.ProjectNameField, 0, 1);
            this.WindowLayoutTable.Controls.Add(this.ProjectDescField, 0, 4);
            this.WindowLayoutTable.Controls.Add(this.ProjectName, 0, 0);
            this.WindowLayoutTable.Controls.Add(this.ProjectDesc, 0, 3);
            this.WindowLayoutTable.Controls.Add(this.ConfirmButton, 0, 6);
            this.WindowLayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowLayoutTable.Location = new System.Drawing.Point(7, 7);
            this.WindowLayoutTable.Name = "WindowLayoutTable";
            this.WindowLayoutTable.RowCount = 7;
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.WindowLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.WindowLayoutTable.Size = new System.Drawing.Size(459, 449);
            this.WindowLayoutTable.TabIndex = 0;
            // 
            // ProjectNameField
            // 
            this.ProjectNameField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProjectNameField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProjectNameField.Location = new System.Drawing.Point(5, 28);
            this.ProjectNameField.Margin = new System.Windows.Forms.Padding(5);
            this.ProjectNameField.MaxLength = 30;
            this.ProjectNameField.Multiline = false;
            this.ProjectNameField.Name = "ProjectNameField";
            this.ProjectNameField.Size = new System.Drawing.Size(267, 28);
            this.ProjectNameField.TabIndex = 5;
            this.ProjectNameField.Text = "";
            this.ProjectNameField.TextChanged += new System.EventHandler(this.ProjectNameField_TextChanged);
            // 
            // ProjectDescField
            // 
            this.ProjectDescField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProjectDescField.Location = new System.Drawing.Point(5, 109);
            this.ProjectDescField.Margin = new System.Windows.Forms.Padding(5);
            this.ProjectDescField.MaxLength = 165;
            this.ProjectDescField.Name = "ProjectDescField";
            this.ProjectDescField.Size = new System.Drawing.Size(267, 82);
            this.ProjectDescField.TabIndex = 3;
            this.ProjectDescField.Text = "";
            this.ProjectDescField.TextChanged += new System.EventHandler(this.ProjectDescField_TextChanged);
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSize = true;
            this.ProjectName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProjectName.Location = new System.Drawing.Point(0, 5);
            this.ProjectName.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(42, 13);
            this.ProjectName.TabIndex = 0;
            this.ProjectName.Text = "Name *";
            // 
            // ProjectDesc
            // 
            this.ProjectDesc.AutoSize = true;
            this.ProjectDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProjectDesc.Location = new System.Drawing.Point(0, 86);
            this.ProjectDesc.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.ProjectDesc.Name = "ProjectDesc";
            this.ProjectDesc.Size = new System.Drawing.Size(67, 13);
            this.ProjectDesc.TabIndex = 1;
            this.ProjectDesc.Text = "Description *";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConfirmButton.AutoSize = true;
            this.ConfirmButton.FlatAppearance.BorderSize = 0;
            this.ConfirmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConfirmButton.Location = new System.Drawing.Point(162, 307);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(135, 51);
            this.ConfirmButton.TabIndex = 4;
            this.ConfirmButton.Text = "Create";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Visible = false;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // ProjectSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(473, 463);
            this.Controls.Add(this.WindowLayoutTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProjectSettings";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Project Settings";
            this.Load += new System.EventHandler(this.ProjectSettings_Load);
            this.WindowLayoutTable.ResumeLayout(false);
            this.WindowLayoutTable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel WindowLayoutTable;
        private System.Windows.Forms.RichTextBox ProjectDescField;
        private System.Windows.Forms.Label ProjectName;
        private System.Windows.Forms.Label ProjectDesc;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.RichTextBox ProjectNameField;
    }
}