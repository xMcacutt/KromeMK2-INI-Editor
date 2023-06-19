using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ty2INIEditor.INIHandler
{
    internal class INI
    {
        public string Path;

        public int LineCount;

        public int StringTableOffset;

        public int ShortTableOffset;

        public int SectionCount;

        public int DataLength;

        public int HashTableOffset;

        public int BinarySearchTableOffset;

        public int HashDivisor;

        public List<Line> Lines = new List<Line>();
    }
}
