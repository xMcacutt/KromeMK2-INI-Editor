using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class Section 
    {
        public string Name;

        public int LineCount;

        public List<Line> DataLines = new List<Line>();

        public List<Field> Fields = new List<Field>();

        public List<SubSection> SubSections = new List<SubSection>();
    }
}
