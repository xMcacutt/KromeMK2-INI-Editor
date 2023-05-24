using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ty2INIEditor
{
    internal class Line
    {
        public ushort FieldStringCount;

        public ushort SectionNameOffset;

        public ushort FieldNameOffset;

        public ushort RollingFieldStringCount;

        public ushort DataStartLineIndex;

        public ushort MaskNameOffset;

        public bool Handled;

        public string Type;

        public string Text;

        public string[] ChildData;
    }
}
