using System.Collections;
using System.Text;

namespace Tibsoft.BloomFilter.Tests.Extensions
{
    public static class BitArrayExtensions
    {
        public static string ToText(this BitArray array)
        {
            var sb = new StringBuilder();

            foreach (bool b in array)
            {
                sb.Append(b ? "1" : "0");
            }

            return sb.ToString();
        }
    }
}
