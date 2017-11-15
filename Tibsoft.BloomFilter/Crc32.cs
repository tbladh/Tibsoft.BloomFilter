using System;

namespace Tibsoft.BloomFilter
{
    public class Crc32
    {

        public static readonly Crc32 Default = new Crc32();

        private static readonly uint[] Table;

        private readonly uint _xor;

        public uint ComputeChecksum(uint value)
        {
            return ComputeChecksum(BitConverter.GetBytes(value));
        }

        public uint ComputeChecksum(byte[] bytes)
        {
            var crc = 0xffffffff;
            foreach (var t in bytes)
            {
                var index = (byte)((crc & 0xff) ^ t);
                crc = (crc >> 8) ^ Table[index];
            }
            return ~crc ^ _xor;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }

        static Crc32()
        {
            const uint poly = 0xedb88320;
            Table = new uint[256];
            for (uint i = 0; i < Table.Length; ++i)
            {
                var temp = i;
                for (var j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (temp >> 1) ^ poly;
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                Table[i] = temp;
            }
        }

        public Crc32()
        {
        }

        public Crc32(uint xor)
        {
            _xor = xor;
        }

    }
}
