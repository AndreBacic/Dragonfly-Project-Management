using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Bug_Tracker_Library
{
    /// <summary>
    /// Class for converting Guids to and from ints, decimals, and longs
    /// Credit: first answer from https://stackoverflow.com/questions/49372626/convert-guid-to-2-longs-and-2-longs-to-guid-in-c-sharp
    /// </summary>
    public static class GuidTools
    {
        public static Guid GuidFromLongs(long a, long b)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(a), guidData, 8);
            Array.Copy(BitConverter.GetBytes(b), 0, guidData, 8, 8);
            return new Guid(guidData);
        }

        public static (long, long) ToLongs(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var long1 = BitConverter.ToInt64(bytes, 0);
            var long2 = BitConverter.ToInt64(bytes, 8);
            return (long1, long2);
        }

        public static Guid GuidFromULongs(ulong a, ulong b)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(a), guidData, 8);
            Array.Copy(BitConverter.GetBytes(b), 0, guidData, 8, 8);
            return new Guid(guidData);
        }

        public static (ulong, ulong) ToULongs(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var ulong1 = BitConverter.ToUInt64(bytes, 0);
            var ulong2 = BitConverter.ToUInt64(bytes, 8);
            return (ulong1, ulong2);
        }

        public static Guid GuidFromInts(int a, int b, int c, int d)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(a), guidData, 4);
            Array.Copy(BitConverter.GetBytes(b), 0, guidData, 4, 4);
            Array.Copy(BitConverter.GetBytes(c), 0, guidData, 8, 4);
            Array.Copy(BitConverter.GetBytes(d), 0, guidData, 12, 4);
            return new Guid(guidData);
        }

        public static (int, int, int, int) ToInts(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var a = BitConverter.ToInt32(bytes, 0);
            var b = BitConverter.ToInt32(bytes, 4);
            var c = BitConverter.ToInt32(bytes, 8);
            var d = BitConverter.ToInt32(bytes, 12);
            return (a, b, c, d);
        }

        public static Guid GuidFromUInts(uint a, uint b, uint c, uint d)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(a), guidData, 4);
            Array.Copy(BitConverter.GetBytes(b), 0, guidData, 4, 4);
            Array.Copy(BitConverter.GetBytes(c), 0, guidData, 8, 4);
            Array.Copy(BitConverter.GetBytes(d), 0, guidData, 12, 4);
            return new Guid(guidData);
        }

        public static (uint, uint, uint, uint) ToUInts(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var a = BitConverter.ToUInt32(bytes, 0);
            var b = BitConverter.ToUInt32(bytes, 4);
            var c = BitConverter.ToUInt32(bytes, 8);
            var d = BitConverter.ToUInt32(bytes, 12);
            return (a, b, c, d);
        }


        [StructLayout(LayoutKind.Explicit)]
        struct GuidConverter
        {
            [FieldOffset(0)]
            public decimal Decimal;
            [FieldOffset(0)]
            public Guid Guid;
            [FieldOffset(0)]
            public long Long1;
            [FieldOffset(8)]
            public long Long2;
        }

        private static GuidConverter _converter;
        public static (long, long) FastGuidToLongs(this Guid guid)
        {
            _converter.Guid = guid;
            return (_converter.Long1, _converter.Long2);
        }
        public static Guid FastLongsToGuid(long a, long b)
        {
            _converter.Long1 = a;
            _converter.Long2 = b;
            return _converter.Guid;
        }
    }
}
