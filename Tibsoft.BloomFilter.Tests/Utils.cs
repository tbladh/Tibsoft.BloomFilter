using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tibsoft.BloomFilter.Tests
{
    public static class Utils
    {
        public static IEnumerable<string> RandomStrings(int n, uint seed)
        {
            var random = new XorShiftRandom(seed, 10);
            for (var i = 0; i < n; i++)
            {
                yield return RandomString(random, 32);
            }
        }

        public static string RandomString(XorShiftRandom random, int length)
        {
            var bytes = random.NextBytes(length);
            var s = Encoding.UTF8.GetString(bytes.Select(p => (byte)(p % 96 + 32)).ToArray());
            return s;
        }

    }
}
