using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TyBNIEditor.Forms;

namespace TyBNIEditor
{
    internal static class Program
    {
        public static Editor Editor;
        public static Preferences Preferences;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Editor = new Editor());
        }
    }
}
