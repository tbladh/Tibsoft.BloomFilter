using System;

namespace Tibsoft.BloomFilter
{

    public class XorShiftState
    {
        public uint X { get; set; }
        public uint Y { get; set; }
        public uint Z { get; set; }
        public uint W { get; set; }

        public XorShiftState(uint x, uint y, uint z, uint w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public XorShiftState Clone()
        {
            return new XorShiftState(X, Y, Z, W);
        }

    }

    public class XorShiftRandom
    {

        public XorShiftState InitialState { get; }

        public XorShiftState State { get; set; }

        private XorShiftState _savedState = null; 

        public XorShiftRandom()
        {
            State = new XorShiftState(123456789, 362436069, 521288629, 88675123);
            InitialState = State;
        }
        public XorShiftRandom(uint seed, int warmup = 0): this()
        {
            State.W = State.W ^ seed;
            if (warmup <= 0) return;
            Warmup(warmup);
            InitialState = State;
        }

        public void SaveState()
        {
            if (_savedState != null) throw new Exception("State has already been saved.");
            _savedState = State.Clone();
        }

        public void RestoreState()
        {
            State = _savedState;
            _savedState = null;
        }

        public void Reset()
        {
            State = InitialState;
            _savedState = null;
        }

        private void Warmup(int n)
        {
            for (var i = 0; i < n; i++)
            {
                NextDouble();
            }
        }

        public double NextDouble()
        {
            var buffer = NextBytes(4);
            var num = BitConverter.ToUInt32(buffer, 0);
            var d = (double)num / uint.MaxValue;
            return d;
        }

        public int Next(int limit)
        {
            var d = NextDouble();
            return (int)(limit * d);
        }

        public int Next()
        {
            var buffer = NextBytes(4);
            return BitConverter.ToInt32(buffer, 0);
        }

        public uint NextUInt()
        {
            var buffer = NextBytes(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public byte[] NextBytes(int bytes)
        {
            var buffer = new byte[bytes];

            FillBuffer(buffer, 0, bytes);

            return buffer;
        }

        public void FillBuffer(byte[] buf, int offset, int offsetEnd)
        {
            while (offset < offsetEnd)
            {
                var t = State.X ^ (State.X << 11);
                State.X = State.Y; State.Y = State.Z; State.Z = State.W;
                State.W = State.W ^ (State.W >> 19) ^ (t ^ (t >> 8));
                buf[offset++] = (byte)(State.W & 0xFF);
                buf[offset++] = (byte)((State.W >> 8) & 0xFF);
                buf[offset++] = (byte)((State.W >> 16) & 0xFF);
                buf[offset++] = (byte)((State.W >> 24) & 0xFF);
            }
        }
    }
}
