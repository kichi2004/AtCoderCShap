using System.Runtime.CompilerServices;

namespace Solve.Libraries.SegmentTree.LazySegmentTree
{
    public class LazySegmentTree<T, TOperator> where TOperator : IEquatable<TOperator>
    {
        public int Size { get; }

        readonly int _height;

        readonly T[] _data;
        readonly TOperator[] _lazy;

        readonly Func<T, T, T> _f;
        readonly Func<T, TOperator, T> _g;
        readonly Func<TOperator, TOperator, TOperator> _h;
        readonly T _identity;
        readonly TOperator _operatorIdentity;

        public LazySegmentTree(int n,
            Func<T, T, T> f,
            Func<T, TOperator, T> g,
            Func<TOperator, TOperator, TOperator> h,
            T identity = default,
            TOperator operatorIdentity = default
        ) {
            int sz = 1;
            int height = 0;
            while (sz < n) {
                sz <<= 1;
                height++;
            }

            Size = sz;
            _height = height;
            _data = Enumerable.Repeat(identity, 2 * sz).ToArray();
            _lazy = Enumerable.Repeat(operatorIdentity, 2 * sz).ToArray();
            (_f, _g, _h) = (f, g, h);
            _identity = identity;
            _operatorIdentity = operatorIdentity;
        }

        public void Set(int i, T x) => _data[i + Size] = x;

        public void Build() {
            for (int i = Size - 1; i > 0; i--) _data[i] = _f(_data[2 * i + 0], _data[2 * i + 1]);
        }

        [MethodImpl(256)] public void Propagate(int i) {
            if (_lazy[i].Equals(_operatorIdentity)) return;
            _lazy[2 * i + 0] = _h(_lazy[2 * i + 0], _lazy[i]);
            _lazy[2 * i + 1] = _h(_lazy[2 * i + 1], _lazy[i]);
            _data[i] = Reflect(i);
            _lazy[i] = _operatorIdentity;
        }

        [MethodImpl(256)]
        T Reflect(int i) => _lazy[i].Equals(_operatorIdentity) ? _data[i] : _g(_data[i], _lazy[i]);

        [MethodImpl(256)] void Recalc(int i) {
            while ((i >>= 1) > 0) _data[i] = _f(Reflect(2 * i + 0), Reflect(2 * i + 1));
        }

        [MethodImpl(256)] void Thrust(int i) { for (int j = _height; j > 0; j--) Propagate(i >> j); }

        public void Update(int a, int b, TOperator x) {
            Thrust(a += Size);
            Thrust(b += Size - 1);
            for (int l = a, r = b + 1; l < r; l >>= 1, r >>= 1) {
                if ((l & 1) != 0) { _lazy[l] = _h(_lazy[l], x); ++l; }
                if ((r & 1) != 0) { --r; _lazy[r] = _h(_lazy[r], x); }
            }

            Recalc(a);
            Recalc(b);
        }

        public T Query(int a, int b) {
            Thrust(a += Size);
            Thrust(b += Size - 1);
            T L = _identity, R = _identity;
            for (int l = a, r = b + 1; l < r; l >>= 1, r >>= 1) {
                if ((l & 1) != 0) L = _f(L, Reflect(l++));
                if ((r & 1) != 0) R = _f(Reflect(--r), R);
            }

            return _f(L, R);
        }

        public T this[int k] {
            get => Query(k, k + 1);
            set => Set(k, value);
        }

        T Apply(int k) => _lazy[k].Equals(_operatorIdentity) ? _data[k] : _g(_data[k], _lazy[k]);


        int FindSubtree(int a, Func<T, bool> check, T M, bool type) {
            while (a < Size) {
                Propagate(a);
                var nxt = type ? _f(Apply(2 * a + 1), M) : _f(M, Apply(2 * a));
                if (check(nxt)) a = 2 * a + (type ? 1 : 0);
                else {
                    M = nxt;
                    a = 2 * a + 1 - (type ? 1 : 0);
                }
            }

            return a - Size;
        }

        public int FindFirst(int a, Func<T, bool> check) {
            var L = _identity;
            if(a <= 0) {
                if(check(_f(L, Apply(1)))) return FindSubtree(1, check, L, false);
                return -1;
            }
            Thrust(a + Size);
            int b = Size;
            for(a += Size, b += Size; a < b; a >>= 1, b >>= 1) {
                if((a & 1) != 0) {
                    T nxt = _f(L, Apply(a));
                    if(check(nxt)) return FindSubtree(a, check, L, false);
                    L = nxt;
                    ++a;
                }
            }
            return -1;
        }
  
        public int FindLast(int b, Func<T,bool> check) {
            var R = _identity;
            if(b >= Size) {
                if(check(_f(Apply(1), R))) return FindSubtree(1, check, R, true);
                return -1;
            }
            Thrust(b + Size - 1);
            int a = Size;
            for(b += Size; a < b; a >>= 1, b >>= 1) {
                if((b & 1) != 0) {
                    T nxt = _f(Apply(--b), R);
                    if(check(nxt)) return FindSubtree(b, check, R, true);
                    R = nxt;
                }
            }
            return -1;
        }
    }
}