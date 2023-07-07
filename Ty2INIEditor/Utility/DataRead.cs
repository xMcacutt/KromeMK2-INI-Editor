using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Ty2INIEditor
{
    public static class DataRead
    {
        public unsafe static int ToInt32(byte[] value, int startIndex)
        {
            fixed (byte* ptr = &value[startIndex])
            {
                if (SettingsHandler.Settings.LittleEndian)
                {
                    return *ptr | (ptr[1] << 8) | (ptr[2] << 16) | (ptr[3] << 24);
                }

                return (*ptr << 24) | (ptr[1] << 16) | (ptr[2] << 8) | ptr[3];
            }
        }

        public unsafe static byte[] GetBytes(short value)
        {
            byte[] array = new byte[2];
            if (SettingsHandler.Settings.LittleEndian)
            {
                fixed (byte* ptr = array)
                {
                    *(short*)ptr = value;
                }
            }
            else
            {
                fixed (byte* ptr = array)
                {
                    ptr[0] = (byte)(value >> 8);    // Store the high byte (MSB) at index 0
                    ptr[1] = (byte)value;            // Store the low byte (LSB) at index 1
                }
            }
            return array;
        }

        public unsafe static byte[] GetBytes(int value)
        {
            byte[] array = new byte[4];
            if (SettingsHandler.Settings.LittleEndian)
            {
                fixed (byte* ptr = array)
                {
                    *(int*)ptr = value;
                }
            }
            else
            {
                fixed (byte* ptr = array)
                {
                    ptr[0] = (byte)(value >> 24);   // Store the highest byte (MSB) at index 0
                    ptr[1] = (byte)(value >> 16);   // Store the second highest byte at index 1
                    ptr[2] = (byte)(value >> 8);    // Store the second lowest byte at index 2
                    ptr[3] = (byte)value;            // Store the lowest byte (LSB) at index 3
                }
            }
            return array;
        }

        public unsafe static byte[] GetBytes(long value)
        {
            byte[] array = new byte[8];
            if (SettingsHandler.Settings.LittleEndian)
            {
                fixed (byte* ptr = array)
                {
                    *(long*)ptr = value;
                }
            }
            else
            {
                fixed (byte* ptr = array)
                {
                    ptr[0] = (byte)(value >> 56);   
                    ptr[1] = (byte)(value >> 48); 
                    ptr[2] = (byte)(value >> 40);
                    ptr[3] = (byte)(value >> 32);
                    ptr[4] = (byte)(value >> 24);
                    ptr[5] = (byte)(value >> 16);
                    ptr[6] = (byte)(value >> 8);
                    ptr[7] = (byte)value;
                }
            }
            return array;
        }

        public static byte[] GetBytes(ushort value)
        {
            return GetBytes((short)value);
        }

        public static byte[] GetBytes(uint value)
        {
            return GetBytes((int)value);
        }

        public static byte[] GetBytes(ulong value)
        {
            return GetBytes((long)value);
        }

        public unsafe static byte[] GetBytes(float value)
        {
            return GetBytes(*(int*)(&value));
        }

        public unsafe static byte[] GetBytes(double value)
        {
            return GetBytes(*(long*)(&value));
        }

        public static char ToChar(byte[] value, int startIndex)
        {
            return (char)ToInt16(value, startIndex);
        }

        public unsafe static short ToInt16(byte[] value, int startIndex)
        {
            fixed (byte* ptr = &value[startIndex])
            {
                if (SettingsHandler.Settings.LittleEndian)
                {
                    return (short)(value[startIndex] | (value[startIndex + 1] << 8));
                }
                return (short)(value[startIndex + 1] | (value[startIndex] << 8));
            }
        }
        public unsafe static long ToInt64(byte[] value, int startIndex)
        {

            fixed (byte* ptr = &value[startIndex])
            {
                if (SettingsHandler.Settings.LittleEndian)
                {
                    int num = *ptr | (ptr[1] << 8) | (ptr[2] << 16) | (ptr[3] << 24);
                    int num2 = ptr[4] | (ptr[5] << 8) | (ptr[6] << 16) | (ptr[7] << 24);
                    return (uint)num | ((long)num2 << 32);
                }

                int num3 = (*ptr << 24) | (ptr[1] << 16) | (ptr[2] << 8) | ptr[3];
                int num4 = (ptr[4] << 24) | (ptr[5] << 16) | (ptr[6] << 8) | ptr[7];
                return (uint)num4 | ((long)num3 << 32);
            }
        }

        public static unsafe ushort ToUInt16(byte[] value, int startIndex)
        {
            return (ushort)ToInt16(value, startIndex);
        }

        public static uint ToUInt32(byte[] value, int startIndex)
        {
            return (uint)ToInt32(value, startIndex);
        }

        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            return (ulong)ToInt64(value, startIndex);
        }

        public unsafe static float ToSingle(byte[] value, int startIndex)
        {
            int num = ToInt32(value, startIndex);
            return *(float*)(&num);
        }

        public unsafe static double ToDouble(byte[] value, int startIndex)
        {
            long num = ToInt64(value, startIndex);
            return *(double*)(&num);
        }

        public unsafe static long DoubleToInt64Bits(double value)
        {
            return *(long*)(&value);
        }

        public unsafe static double Int64BitsToDouble(long value)
        {
            return *(double*)(&value);
        }
    }
}

