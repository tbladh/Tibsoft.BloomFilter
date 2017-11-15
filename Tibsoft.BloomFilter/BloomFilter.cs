using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tibsoft.BloomFilter
{
    public class BloomFilter<T>
    {
        private static readonly double Ln2 = Math.Log(2);
        private BitArray _filter;
        public int Capacity => _filter.Length;
        public int Bits => _filter.Length * 8;

        public Encoding Encoding { get; private set; }

        private HashFunctions _hashFunctions;

        public void Add(T item)
        {
            for (var i = 0; i < _hashFunctions.Count; i++)
            {
                var bit = _hashFunctions.Calculate(i, item);
                _filter.Set(bit, true);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool Contains(T item)
        {
            // If any of the bit locations returned by the hash functions is unset then 
            // the item is definitely not in the set (since that bit would have been set 
            // on add). If all the bit locations are set however then there is still a 
            // chance this is by accident. 
            for (var i = 0; i < _hashFunctions.Count; i++)
            {
                var bit = _hashFunctions.Calculate(i, item);
                var found = _filter.Get((int)bit);
                if (!found) return false; // Definitely not in the set.
            }
            return true; // Possible false positive by design.
        }

        private void Initialize(int capacity, int bitsPerItem, int hashFunctions, Encoding encoding)
        {
            if (capacity < 0) throw new ArgumentException($"'{nameof(capacity)}' cannot be negative.");
            if (bitsPerItem < 0) throw new ArgumentException($"'{nameof(bitsPerItem)}' cannot be negative.");
            if (hashFunctions < 0) throw new ArgumentException($"'{nameof(hashFunctions)}' cannot be negative.");
            Encoding = encoding;
            var bits = capacity * bitsPerItem;
            var rem = bits % 8; // Adjust bits to multiple of 8 > bits;
            bits = bits + (rem > 0 ? (8 - rem) : 0);
            _filter = new BitArray(bits);
            _hashFunctions = new HashFunctions(hashFunctions, bits, Encoding);
        }

        public BloomFilter(int capacity, int bitsPerItem = 14, Encoding encoding = null)
        {
            Encoding = encoding;
            var bits = capacity * bitsPerItem;
            var hashCount = (int)Math.Ceiling((bits / (double)capacity) * Ln2);
            Initialize(capacity, bitsPerItem, hashCount, encoding);
        }
    }
}
