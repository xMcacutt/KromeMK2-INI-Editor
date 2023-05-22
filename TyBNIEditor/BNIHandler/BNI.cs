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

        public int ShortTableOffset;

        public int StringTableEndOffset;

        public int SectionCount;

        public int DataLength;

        public List<Line> Lines = new List<Line>();

        public List<Section> Sections = new List<Section>();

        public HashSet<string> StringHashSet = new HashSet<string>();

        public Dictionary<string, int> StringDictionary = new Dictionary<string, int>();
    }
}
