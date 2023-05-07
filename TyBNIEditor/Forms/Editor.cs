using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;
using TyBNIEditor.Forms;

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
            Fonts.Setup();
            Themes.Load();
            SettingsHandler.Setup();
            InitializeComponent();
            popupMenu = new AutocompleteMenu(FCTB);
            InitializeColors();
            InitializeFonts();
            string[] SectionNames = File.ReadAllLines("./Data/SectionNames.txt");
            _sectionNamesRegexExp = @"^\b(" + string.Join("|", SectionNames.Select(sn => Regex.Escape(sn))) + @")\b";
            string[] FieldNames = File.ReadAllLines("./Data/FieldNames.txt");
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

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Program.Preferences == null) Program.Preferences = new Preferences();
            Program.Preferences.Show();
        }
    }
}
