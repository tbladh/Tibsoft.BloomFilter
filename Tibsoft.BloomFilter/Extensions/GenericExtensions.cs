using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tibsoft.BloomFilter.Extensions
{
    public static class GenericExtensions
    {
        public static byte[] GetBytes<T>(this T value, Encoding encoding = null)
        {
            // TODO: Likely very slow.
            encoding = encoding ?? Encoding.UTF8;

            var s = value as string;
            if (s != null) return encoding.GetBytes(s);
            var b = value as byte[];
            if (b != null) return b;

            if (value.GetType().IsValueType)
            {
                if (value is int) return BitConverter.GetBytes((int)(object)value);
                if (value is uint) return BitConverter.GetBytes((uint)(object)value);
                if (value is long) return BitConverter.GetBytes((long)(object)value);
                if (value is ulong) return BitConverter.GetBytes((ulong)(object)value);
                if (value is double) return BitConverter.GetBytes((double)(object)value);
                if (value is float) return BitConverter.GetBytes((float)(object)value);
                if (value is short) return BitConverter.GetBytes((short)(object)value);
                if (value is ushort) return BitConverter.GetBytes((ushort)(object)value);
                if (value is char) return BitConverter.GetBytes((char)(object)value);
                if (value is byte) return new[] { (byte)(object)value };
                // etc...
            }

            var e = value as IEnumerable<byte>;
            return e?.ToArray() ?? BitConverter.GetBytes(value.GetHashCode());
        }
    }
}
