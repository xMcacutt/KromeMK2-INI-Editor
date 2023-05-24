using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ty2INIEditor
{
    internal class Colors
    {
        public int[] MainTextRGB { get; set; }
        public int[] BackgroundDarkRGB { get; set; }
        public int[] BackgroundLightRGB { get; set; }
        public int[] BackgroundSuperLightRGB { get; set; }
        public int[] FieldNamesRGB { get; set; }
        public int[] FieldTextRGB { get; set; }
        public int[] SectionNamesRGB { get; set; }
        public int[] NumbersRGB { get; set; }
        public int[] KeywordsRGB { get; set; }

        public Color MainText;
        public Color BackgroundDark;
        public Color BackgroundLight;
        public Color BackgroundSuperLight;
        public Color FieldNames;
        public Color FieldText;
        public Color SectionNames;
        public Color Numbers;
        public Color Keywords;

        public void Setup()
        {
            MainText = Color.FromArgb(MainTextRGB[0], MainTextRGB[1], MainTextRGB[2]);
            BackgroundDark = Color.FromArgb(BackgroundDarkRGB[0], BackgroundDarkRGB[1], BackgroundDarkRGB[2]);
            BackgroundLight = Color.FromArgb(BackgroundLightRGB[0], BackgroundLightRGB[1], BackgroundLightRGB[2]);
            BackgroundSuperLight = Color.FromArgb(BackgroundSuperLightRGB[0], BackgroundSuperLightRGB[1], BackgroundSuperLightRGB[2]);
            FieldNames = Color.FromArgb(FieldNamesRGB[0], FieldNamesRGB[1], FieldNamesRGB[2]);
            FieldText = Color.FromArgb(FieldTextRGB[0], FieldTextRGB[1], FieldTextRGB[2]);
            SectionNames = Color.FromArgb(SectionNamesRGB[0], SectionNamesRGB[1], SectionNamesRGB[2]);
            Numbers = Color.FromArgb(NumbersRGB[0], NumbersRGB[1], NumbersRGB[2]);
            Keywords = Color.FromArgb(KeywordsRGB[0], KeywordsRGB[1], KeywordsRGB[2]);
        }
    }
}
