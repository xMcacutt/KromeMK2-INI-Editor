using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class SubSection
    {
        public string Name;
        public int StartLineIndex;
        public int Index;
        public List<Field> Fields = new List<Field>();
    }
}
