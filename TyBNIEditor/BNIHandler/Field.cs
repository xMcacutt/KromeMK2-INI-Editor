using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class Field
    {
        public string Name;
        public bool IsSubSection;
        public int SubSectionIndex;
        public List<string> Strings = new List<string>();
    }
}
