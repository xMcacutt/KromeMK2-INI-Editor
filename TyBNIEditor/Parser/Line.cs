using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    internal class Line
    {
        public short StringCount;
        public ushort SectionNameOffset;
        public ushort FieldNameOffset1;
        public ushort Index;
        public ushort FieldNameOffset2;
        public List<string> Strings = new List<string>();
        public string SectionName;
        public string FieldName;

        public static implicit operator Line(FastColoredTextBoxNS.Line v)
        {
            throw new NotImplementedException();
        }
    }
}
