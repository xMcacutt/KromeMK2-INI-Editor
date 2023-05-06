using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System;

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
using System.Runtime.CompilerServices;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

namespace TyBNIEditor
{
    public partial class Editor : Form
    {
        string _sectionNamesRegexExp;
        string _fieldNamesRegexExp;
        public TextStyle KeywordsStyle;
        public TextStyle SectionNamesStyle;
        public TextStyle FieldNamesStyle;
        public TextStyle NumbersStyle;
        public TextStyle FieldTextStyle;
        AutocompleteMenu popupMenu;

        public Editor()
        {
            InitializeComponent();
            SettingsHandler.Setup();
            popupMenu = new AutocompleteMenu(FCTB);
            InitializeColors();
            string[] SectionNames = File.ReadAllLines("./SectionNames.txt");
            _sectionNamesRegexExp = @"^\b(" + string.Join("|", SectionNames.Select(sn => Regex.Escape(sn))) + @")\b";
            string[] FieldNames = File.ReadAllLines("./FieldNames.txt");
            _fieldNamesRegexExp += @"^\b(" + string.Join("|", FieldNames.Select(fn => Regex.Escape(fn))) + @")\b";

            popupMenu.MinFragmentLength = 2;
            popupMenu.Items.SetAutocompleteItems(SectionNames.Concat(FieldNames).ToArray());
            popupMenu.Items.MaximumSize = new Size(300, 400);
            popupMenu.Items.Width = 300;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileSelect = new OpenFileDialog
            {
                Filter = "LV3 Files (.lv3)|*.LV3|BNI Files (.bni)|*.bni"
            };
            DialogResult result = fileSelect.ShowDialog();
            if (result != DialogResult.OK) return;
            string path = fileSelect.FileName;
            FileNameLabel.Text = Path.GetFileName(path);
            FCTB.Text = string.Join("\n", Parser.Import(path));
        }

        public void InitializeColors()
        {
            popupMenu.BackColor = SettingsHandler.Colors.BackgroundSuperLight;
            popupMenu.ForeColor = SettingsHandler.Colors.MainText;
            popupMenu.HoveredColor = SettingsHandler.Colors.BackgroundLight;
            popupMenu.SelectedColor = SettingsHandler.Colors.BackgroundLight;
            popupMenu.Font = new Font("Cascadia Code", 12F);
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
            KeywordsStyle = new TextStyle(keywordsBrush, null, FontStyle.Regular);
            Brush sectionNamesBrush = new SolidBrush(SettingsHandler.Colors.SectionNames);
            SectionNamesStyle = new TextStyle(sectionNamesBrush, null, FontStyle.Regular);
            Brush fieldNamesBrush = new SolidBrush(SettingsHandler.Colors.FieldNames);
            FieldNamesStyle = new TextStyle(fieldNamesBrush, null, FontStyle.Regular);
            Brush fieldTextBrush = new SolidBrush(SettingsHandler.Colors.FieldText);
            FieldTextStyle = new TextStyle(fieldTextBrush, null, FontStyle.Regular);
            Brush numbersBrush = new SolidBrush(SettingsHandler.Colors.Numbers);
            NumbersStyle = new TextStyle(numbersBrush, null, FontStyle.Regular);
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
            if (_sectionNamesRegexExp != null) e.ChangedRange.SetStyle(SectionNamesStyle, _sectionNamesRegexExp, RegexOptions.Multiline);
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
    }
}
