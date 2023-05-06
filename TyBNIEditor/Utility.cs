using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyBNIEditor
{
    public static class Utility
    {
        public static string ReadString(byte[] bytes, int position)
        {
            int endOfString = Array.IndexOf<byte>(bytes, 0x0, position);
            while(endOfString == position) 
            { 
                position += 1;
                endOfString = Array.IndexOf<byte>(bytes, 0x0, position);
            }
            return Encoding.UTF8.GetString(bytes, position, endOfString - position);
        }
    }
}
