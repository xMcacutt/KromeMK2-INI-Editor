using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TyBNIEditor
{
    internal class Fonts
    {
        public static Font Standard;
        public static Font SmallUI;
        public static Font MediumUI;
        public static Font LargeUI;

        public static void Setup()
        {
            Standard = new Font("Cascadia Code", 10F);
            SmallUI = new Font("Cascadia Code", 8F);
            MediumUI = new Font("Cascadia Code", 12F);
            LargeUI = new Font("Cascadia Code", 16F);
        }

        public static void SetFonts(Font font)
        {
            Standard = new Font(font.FontFamily, 10F);
            SmallUI = new Font(font.FontFamily, 8F);
            MediumUI = new Font(font.FontFamily, 12F);
            LargeUI = new Font(font.FontFamily, 16F);
        }
    }
}
