using System.Text;
using Tibsoft.BloomFilter.Extensions;

namespace Tibsoft.BloomFilter
{
    public class HashFunctions
    {

        public int Count { get; set; }

        public int Bits { get; set; }

        public Encoding Encoding { get; set; }

        private readonly Crc32[] _functions;

        public int Calculate<T>(int function, T value) 
        {
            var random = _functions[function];
            var input = value.GetBytes(Encoding);
            var result = (int)random.ComputeChecksum(input).Fit((uint)Bits);
            return result;
        }

        public HashFunctions(int count, int bits = 16, Encoding encoding = null)
        {
            Encoding = encoding ?? Encoding.UTF8;
            Bits = bits;
            Count = count;
            // Using a custom deterministic random number generator here on the off 
            // chance that Microsoft changes the implementation of Random sometime
            // in the future.
            var root = new XorShiftRandom(1234, 10);
            _functions = new Crc32[Count];
            for (var i = 0; i < _functions.Length; i++)
            {
                _functions[i] = new Crc32(root.NextUInt());
            }
        }

    }
}
