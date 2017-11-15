using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using Tibsoft.BloomFilter.Tests.Extensions;

namespace Tibsoft.BloomFilter.Tests
{
    [TestFixture]
    public class HashFunctionsTests
    {
        [Test]
        public void GetBitPosition_OverflowTest()
        {
            const int bpi = 14;
            const int k = (int)(bpi * 0.7);
            const int items = 10;
            const int bits = items*bpi;
            
            var array = new BitArray(bits);
            var hashFunctions = new HashFunctions(k, 16);

            for (var i = 0; i < items; i++)
            {
                var item = Guid.NewGuid().ToString();
                for(var j = 0; j < hashFunctions.Count; j++)
                {
                    var bit = hashFunctions.Calculate(j, item);
                    array.Set((int)bit, true);
                }

                Debug.WriteLine(array.ToText());
            }
            
        }
    }
}
