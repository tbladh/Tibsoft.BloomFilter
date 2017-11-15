namespace Tibsoft.BloomFilter.Extensions
{
    public static class NumberExtensions
    {
        public static uint Fit(this uint value, uint limit)
        {
            return value % limit;
        }
    }
}
