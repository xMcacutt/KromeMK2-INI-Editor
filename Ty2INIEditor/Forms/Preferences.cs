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
    public partial class Preferences : Form
    {
        public bool SafeToClose = true;
        public Preferences()
        {
            InitializeComponent();
            InitializeColors();
            InitializeFonts();
            InitializeThemes();
        }

        private void InitializeThemes()
        {
            ThemeSelector.Items.Clear();
            ThemeSelector.Items.AddRange(Themes.ThemeNames.ToArray());
            ThemeSelector.Items.Add("Custom..");
        }

        public void InitializeColors()
        {
            BackColor = SettingsHandler.Colors.BackgroundDark;
            PreferencesLabel.ForeColor = SettingsHandler.Colors.MainText;
            WindowLayout.BackColor = SettingsHandler.Colors.BackgroundDark;
            ColorTablePanel.BackColor = SettingsHandler.Colors.BackgroundLight;
            ColorTableBorder.ForeColor = SettingsHandler.Colors.MainText;
            ColorTableBorder.BackColor = SettingsHandler.Colors.BackgroundLight;
            ColorsLabel.ForeColor = SettingsHandler.Colors.MainText;
            MainTextColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            BackgroundSuperLightColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            BackgroundLightColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            BackgroundDarkColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            FieldNamesColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            FieldTextColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            KeywordsColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            NumbersColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            SectionNamesColorLabel.ForeColor = SettingsHandler.Colors.MainText;
            MainTextColor.BackColor = SettingsHandler.Colors.MainText;
            BackgroundSuperLightColor.BackColor = SettingsHandler.Colors.BackgroundSuperLight;
            BackgroundLightColor.BackColor = SettingsHandler.Colors.BackgroundLight;
            BackgroundDarkColor.BackColor = SettingsHandler.Colors.BackgroundDark;
            FieldNamesColor.BackColor = SettingsHandler.Colors.FieldNames;
            FieldTextColor.BackColor = SettingsHandler.Colors.FieldText;
            KeywordsColor.BackColor = SettingsHandler.Colors.Keywords;
            NumbersColor.BackColor = SettingsHandler.Colors.Numbers;
            SectionNamesColor.BackColor = SettingsHandler.Colors.SectionNames;
            ThemeSelector.BackColor = SettingsHandler.Colors.BackgroundLight;
            ThemeSelector.ForeColor = SettingsHandler.Colors.MainText;
            ThemeSelector.ButtonColor = SettingsHandler.Colors.BackgroundSuperLight;
            ThemeSelector.BorderColor = SettingsHandler.Colors.MainText;
            ThemeLabel.ForeColor = SettingsHandler.Colors.MainText;
            SaveThemeButton.BackColor = SettingsHandler.Colors.BackgroundSuperLight;
            SaveThemeButton.ForeColor = SettingsHandler.Colors.MainText;
            AcceptButton.BackColor = SettingsHandler.Colors.BackgroundSuperLight;
            AcceptButton.ForeColor = SettingsHandler.Colors.MainText;
        }

        private void InitializeFonts()
        {
            PreferencesLabel.Font = Fonts.LargeUI;
            BackgroundDarkColorLabel.Font = Fonts.SmallUI;
            BackgroundLightColorLabel.Font = Fonts.SmallUI;
            BackgroundSuperLightColorLabel.Font = Fonts.SmallUI;
            FieldNamesColorLabel.Font = Fonts.SmallUI;
            MainTextColorLabel.Font = Fonts.SmallUI;
            NumbersColorLabel.Font = Fonts.SmallUI;
            SectionNamesColorLabel.Font = Fonts.SmallUI;
            FieldTextColorLabel.Font = Fonts.SmallUI;
            KeywordsColorLabel.Font = Fonts.SmallUI;
            ColorsLabel.Font = Fonts.MediumUI;
            ThemeLabel.Font = Fonts.MediumUI;
            ThemeSelector.Font = Fonts.Standard;
            SaveThemeButton.Font = Fonts.SmallUI;
            AcceptButton.Font = Fonts.MediumUI;
        }

        private void MainTextColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = MainTextColor.BackColor;
            ColorSelector.ShowDialog();
            if (MainTextColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.MainTextRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            MainTextColor.BackColor = ColorSelector.Color;
        }

        private void BackgroundSuperLightColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = BackgroundSuperLightColor.BackColor;
            ColorSelector.ShowDialog();
            if (BackgroundSuperLightColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.BackgroundSuperLightRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            BackgroundSuperLightColor.BackColor = ColorSelector.Color;
        }

        private void BackgroundLightColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = BackgroundLightColor.BackColor;
            ColorSelector.ShowDialog();
            if (BackgroundLightColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.BackgroundLightRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            BackgroundLightColor.BackColor = ColorSelector.Color;
        }

        private void BackgroundDarkColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = BackgroundDarkColor.BackColor;
            ColorSelector.ShowDialog();
            if (BackgroundDarkColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.BackgroundDarkRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            BackgroundDarkColor.BackColor = ColorSelector.Color;
        }

        private void SectionNamesColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = SectionNamesColor.BackColor;
            ColorSelector.ShowDialog();
            if (SectionNamesColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.SectionNamesRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            SectionNamesColor.BackColor = ColorSelector.Color;
        }

        private void FieldNamesColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = FieldNamesColor.BackColor;
            ColorSelector.ShowDialog();
            if (FieldNamesColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.FieldNamesRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            FieldNamesColor.BackColor = ColorSelector.Color;
        }

        private void FieldTextColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = FieldTextColor.BackColor;
            ColorSelector.ShowDialog();
            if (FieldTextColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.FieldTextRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            FieldTextColor.BackColor = ColorSelector.Color;
        }

        private void KeywordsColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = KeywordsColor.BackColor;
            ColorSelector.ShowDialog();
            if (KeywordsColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.KeywordsRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            KeywordsColor.BackColor = ColorSelector.Color;
        }

        private void NumbersColor_Click(object sender, EventArgs e)
        {
            ColorSelector.Color = NumbersColor.BackColor;
            ColorSelector.ShowDialog();
            if (NumbersColor.BackColor != ColorSelector.Color)
            {
                ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf("Custom..");
            }
            SettingsHandler.Colors.NumbersRGB = new int[] { ColorSelector.Color.R, ColorSelector.Color.G, ColorSelector.Color.B };
            SettingsHandler.Colors.Setup();
            Program.Editor.InitializeColors();
            Program.Preferences.InitializeColors();
            NumbersColor.BackColor = ColorSelector.Color;
        }

        private void ThemeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SafeToClose = false;
            if (ThemeSelector.SelectedIndex == ThemeSelector.Items.IndexOf("Custom.."))
            {
                SaveThemeButton.Visible = true;
                AcceptButton.Enabled = false;
            }
            else
            {
                SaveThemeButton.Visible = false;
                AcceptButton.Enabled = true;
                SettingsHandler.Load(ThemeSelector.SelectedItem.ToString());
            }
        }

        private void SaveThemeButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes"),
                Filter = "Theme json (*.json)|*.json",
                Title = "Save Theme"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = saveFileDialog.FileName;
                string fileName = Path.GetFileNameWithoutExtension(selectedFilePath);
                SettingsHandler.Save(fileName);
                if (Path.GetDirectoryName(selectedFilePath) == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes"))
                {
                    if (!Themes.ThemeNames.Contains(fileName))
                    {
                        Themes.ThemeNames.Add(fileName);
                        InitializeThemes();
                    }
                    ThemeSelector.SelectedIndex = ThemeSelector.Items.IndexOf(fileName);
                }
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            SettingsHandler.Accept();
            SafeToClose = true;
            Hide();
        }

        private void Preferences_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SafeToClose) return;
            SettingsHandler.Load("Colors");
        }
    }
}
