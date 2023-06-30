using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ty2INIEditor
{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asTestRKVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asINIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchAppendCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.FileTC = new TradeWright.UI.Forms.TabControlExtra();
            this.projectManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(56)))));
            this.Menu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(170)))), ((int)(((byte)(200)))));
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(959, 24);
            this.Menu.TabIndex = 0;
            this.Menu.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.saveToProjectToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveToolStripMenuItem.Text = "Save Text";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asTestRKVToolStripMenuItem,
            this.asINIToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // asTestRKVToolStripMenuItem
            // 
            this.asTestRKVToolStripMenuItem.Name = "asTestRKVToolStripMenuItem";
            this.asTestRKVToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.asTestRKVToolStripMenuItem.Text = "As Test RKV";
            this.asTestRKVToolStripMenuItem.Click += new System.EventHandler(this.asTestRKVToolStripMenuItem_Click);
            // 
            // asINIToolStripMenuItem
            // 
            this.asINIToolStripMenuItem.Name = "asINIToolStripMenuItem";
            this.asINIToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.asINIToolStripMenuItem.Text = "As INI";
            this.asINIToolStripMenuItem.Click += new System.EventHandler(this.asINIToolStripMenuItem_Click);
            // 
            // saveToProjectToolStripMenuItem
            // 
            this.saveToProjectToolStripMenuItem.Enabled = false;
            this.saveToProjectToolStripMenuItem.Name = "saveToProjectToolStripMenuItem";
            this.saveToProjectToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveToProjectToolStripMenuItem.Text = "Save To Project";
            this.saveToProjectToolStripMenuItem.Click += new System.EventHandler(this.saveToProjectToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.projectManagerToolStripMenuItem,
            this.batchAppendCurrentToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // batchAppendCurrentToolStripMenuItem
            // 
            this.batchAppendCurrentToolStripMenuItem.Name = "batchAppendCurrentToolStripMenuItem";
            this.batchAppendCurrentToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.batchAppendCurrentToolStripMenuItem.Text = "Batch Append Current";
            this.batchAppendCurrentToolStripMenuItem.Click += new System.EventHandler(this.batchAppendCurrentToolStripMenuItem_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.FileTC);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 24);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(15);
            this.MainPanel.Size = new System.Drawing.Size(959, 533);
            this.MainPanel.TabIndex = 1;
            // 
            // FileTC
            // 
            this.FileTC.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.FileTC.DisplayStyle = TradeWright.UI.Forms.TabStyle.Rounded;
            // 
            // 
            // 
            this.FileTC.DisplayStyleProvider.BlendStyle = TradeWright.UI.Forms.BlendStyle.Normal;
            this.FileTC.DisplayStyleProvider.BorderColorDisabled = System.Drawing.Color.Transparent;
            this.FileTC.DisplayStyleProvider.BorderColorFocused = System.Drawing.Color.Transparent;
            this.FileTC.DisplayStyleProvider.BorderColorHighlighted = System.Drawing.Color.Transparent;
            this.FileTC.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.Transparent;
            this.FileTC.DisplayStyleProvider.BorderColorUnselected = System.Drawing.Color.Transparent;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorFocused = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorFocusedActive = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorHighlighted = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorHighlightedActive = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorSelected = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorSelectedActive = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonFillColorUnselected = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorFocused = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorFocusedActive = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorHighlighted = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorHighlightedActive = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorSelected = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorSelectedActive = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserButtonOutlineColorUnselected = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.CloserColorFocused = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.CloserColorFocusedActive = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.CloserColorHighlighted = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.CloserColorHighlightedActive = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.CloserColorSelected = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.CloserColorSelectedActive = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.CloserColorUnselected = System.Drawing.Color.Empty;
            this.FileTC.DisplayStyleProvider.FocusTrack = false;
            this.FileTC.DisplayStyleProvider.HotTrack = true;
            this.FileTC.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FileTC.DisplayStyleProvider.Opacity = 1F;
            this.FileTC.DisplayStyleProvider.Overlap = 0;
            this.FileTC.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 3);
            this.FileTC.DisplayStyleProvider.PageBackgroundColorDisabled = System.Drawing.SystemColors.Control;
            this.FileTC.DisplayStyleProvider.PageBackgroundColorFocused = System.Drawing.SystemColors.ControlLight;
            this.FileTC.DisplayStyleProvider.PageBackgroundColorHighlighted = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.FileTC.DisplayStyleProvider.PageBackgroundColorSelected = System.Drawing.SystemColors.ControlLightLight;
            this.FileTC.DisplayStyleProvider.PageBackgroundColorUnselected = System.Drawing.SystemColors.Control;
            this.FileTC.DisplayStyleProvider.Radius = 10;
            this.FileTC.DisplayStyleProvider.SelectedTabIsLarger = true;
            this.FileTC.DisplayStyleProvider.ShowTabCloser = true;
            this.FileTC.DisplayStyleProvider.TabColorDisabled1 = System.Drawing.SystemColors.Control;
            this.FileTC.DisplayStyleProvider.TabColorDisabled2 = System.Drawing.SystemColors.Control;
            this.FileTC.DisplayStyleProvider.TabColorFocused1 = System.Drawing.SystemColors.ControlLight;
            this.FileTC.DisplayStyleProvider.TabColorFocused2 = System.Drawing.SystemColors.ControlLight;
            this.FileTC.DisplayStyleProvider.TabColorHighLighted1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.FileTC.DisplayStyleProvider.TabColorHighLighted2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(237)))), ((int)(((byte)(252)))));
            this.FileTC.DisplayStyleProvider.TabColorSelected1 = System.Drawing.SystemColors.ControlLightLight;
            this.FileTC.DisplayStyleProvider.TabColorSelected2 = System.Drawing.SystemColors.ControlLightLight;
            this.FileTC.DisplayStyleProvider.TabColorUnSelected1 = System.Drawing.SystemColors.Control;
            this.FileTC.DisplayStyleProvider.TabColorUnSelected2 = System.Drawing.SystemColors.Control;
            this.FileTC.DisplayStyleProvider.TabPageMargin = new System.Windows.Forms.Padding(1);
            this.FileTC.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.FileTC.DisplayStyleProvider.TextColorFocused = System.Drawing.SystemColors.ControlText;
            this.FileTC.DisplayStyleProvider.TextColorHighlighted = System.Drawing.SystemColors.ControlText;
            this.FileTC.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.FileTC.DisplayStyleProvider.TextColorUnselected = System.Drawing.SystemColors.ControlText;
            this.FileTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTC.HotTrack = true;
            this.FileTC.Location = new System.Drawing.Point(15, 15);
            this.FileTC.Margin = new System.Windows.Forms.Padding(0);
            this.FileTC.Name = "FileTC";
            this.FileTC.SelectedIndex = 0;
            this.FileTC.Size = new System.Drawing.Size(929, 503);
            this.FileTC.TabIndex = 4;
            this.FileTC.TabIndexChanged += new System.EventHandler(this.FileTC_TabIndexChanged);
            // 
            // projectManagerToolStripMenuItem
            // 
            this.projectManagerToolStripMenuItem.Name = "projectManagerToolStripMenuItem";
            this.projectManagerToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.projectManagerToolStripMenuItem.Text = "Project Manager";
            this.projectManagerToolStripMenuItem.Click += new System.EventHandler(this.projectManagerToolStripMenuItem_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(959, 557);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.Menu;
            this.Name = "Editor";
            this.Text = "Ty2 INI Editor";
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asTestRKVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asINIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchAppendCurrentToolStripMenuItem;
        private Panel MainPanel;
        public TradeWright.UI.Forms.TabControlExtra FileTC;
        private ToolStripMenuItem saveToProjectToolStripMenuItem;
        private ToolStripMenuItem projectManagerToolStripMenuItem;
    }
}