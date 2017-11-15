using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Tibsoft.BloomFilter.Tests
{
    [TestFixture]
    public class BloomFilterTests
    {
        [TestCase(24, 0.00002)]
        [TestCase(16, 0.00061)]
        [TestCase(8, 0.02192)]
        [TestCase(6, 0.05598)]
        public void Contains_FalsePositiveTest_ShouldBeWithinAcceptableTolerance(int bitsPerItem, double expectedRate)
        {
            const int testSize = 100000;
            var set = new HashSet<string>();
            var strs = Utils.RandomStrings(10000, 1234);
            foreach (var str in strs)
            {
                set.Add(str);
            }
            var bloom = new BloomFilter<string>(set.Count, bitsPerItem);
            foreach (var item in set)
            {
                bloom.Add(item);
            }
            var falsePositive = 0;
            strs = Utils.RandomStrings(testSize, 4321);
            foreach (var sample in strs)
            {
                if (bloom.Contains(sample) && !set.Contains(sample))
                {
                    falsePositive++;
                }
            }
            var rate = falsePositive / (double) testSize;
            Assert.That(rate, Is.LessThanOrEqualTo(expectedRate));
            Console.WriteLine($"False positive rate: {rate}");
        }

        [Test]
        public void Contains_FalseNegativeTest_ThereShouldBeNone()
        {
            // This test should not be needed unless someone 
            // messed up real bad.
            const int testCount = 100000;
            const uint seed = 1234;
            var strs = Utils.RandomStrings(testCount, seed);
            var bloom = new BloomFilter<string>(testCount, 18);
            foreach (var str in strs)
            {
                bloom.Add(str);
            }
            foreach (var item in Utils.RandomStrings(testCount, seed))
            {
                Assert.That(bloom.Contains(item));
            }
        }

    }
}
