using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class BNI
    {
        public string BNIPath;

        public int LineCount;

        public int StringTableOffset;

        public int ShortTable1Offset;

        public int ShortTable2Offset;

        public int SectionCount;

        public int DataLength;

        public List<Line> Lines = new List<Line>();

        public List<Section> Sections = new List<Section>();
    }
}
