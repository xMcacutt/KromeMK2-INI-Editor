using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class SubSection : Field
    {
        public List<Field> Fields = new List<Field>();

        public List<SubSection> SubSections = new List<SubSection>();

        public ushort RollingStringCountStart;

        public ushort MarkerLineNumber;
    }
}
