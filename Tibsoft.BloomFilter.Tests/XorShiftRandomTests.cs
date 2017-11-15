using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tibsoft.BloomFilter.Tests
{
    [TestFixture]
    public class XorShiftRandomTests
    {
        [Test]
        public void Next_WithDifferentSeed_ShouldReturnDifferentResult()
        {
            // TODO: This is not a test for randomness. Simply testing that different seeds result in mostly different values returned.
            var random1 = new XorShiftRandom(1234);
            var random2 = new XorShiftRandom(4321);

            const int count = 10000;
            var diffCount = 0;
            for (var i = 0; i < count; i++)
            {
                var next1 = random1.Next();
                var next2 = random2.Next();
                if (next1 != next2)
                {
                    diffCount++;
                }

            }
            var similarity = (count - diffCount) / (double)count;

            Assert.That(similarity < 0.0001);

        }

        [TestCase(256)]
        [TestCase(65536)]
        public void Next_WithLimit_ShouldReturnValueWithinRange(int limit)
        {
            var random = new XorShiftRandom(1234);

            var count = 100000;
            for (var i = 0; i < count; i++)
            {
                var value = random.Next(limit);
                Assert.That(value, Is.LessThan(limit));
                Assert.That(value, Is.GreaterThanOrEqualTo(0));
                Debug.WriteLine(value);
            }

        }

    }
}
