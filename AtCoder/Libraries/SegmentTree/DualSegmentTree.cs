using System;
using System.Runtime.CompilerServices;
using static Solve.Methods;

namespace Solve.Libraries.SegmentTree.DualSegmentTree
{
    public class DualSegmentTree<T> where T : IEquatable<T>
    {
        public int Count { get; }
        readonly int _height;
        readonly T[] _lazy;
        readonly Func<T, T, T> _func;
        readonly T _identity;

        public DualSegmentTree(int n, Func<T, T, T> func, T identity = default) {
            (_func, _identity, Count, _height) = (func, identity, 1, 0);
            while (Count < n) {
                Count <<= 1;
                ++_height;
            }

            _lazy = InitArray(Count * 2, _ => identity);
        }

        [MethodImpl(256)] void Propagate(int k) {
            if (_lazy[k].Equals(_identity)) return;
            _lazy[k * 2] = _func(_lazy[2 * k], _lazy[k]);
            _lazy[k * 2 + 1] = _func(_lazy[2 * k + 1], _lazy[k]);
            _lazy[k] = _identity;
        }

        [MethodImpl(256)] void Thrust(int k) {
            for(int i = _height; i > 0; --i) Propagate(k >> i);
        }

        public void Update(int a, int b, T value) {
            Thrust(a += Count);
            Thrust(b += Count - 1);

            for (int l = a, r = b + 1; l < r; l >>= 1, r >>= 1) {
                if ((l & 1) != 0) { _lazy[l] = _func(_lazy[l], value); ++l; }
                if ((r & 1) != 0) { --r; _lazy[r] = _func(_lazy[r], value); }
            }
        }

        public T this[int k] {
            get { Thrust(k += Count); return _lazy[k]; }
        }
    }
}