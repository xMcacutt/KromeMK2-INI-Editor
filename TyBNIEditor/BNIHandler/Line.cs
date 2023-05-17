using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class Line
    {
        public ushort FieldStringCount;

        public ushort SectionNameOffset;

        public ushort FieldNameOffset;

        public ushort RollingFieldStringCount;

        public ushort DataStartLineIndex;
    }
}
