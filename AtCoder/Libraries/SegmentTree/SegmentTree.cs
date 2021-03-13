using System;
using System.Linq;

namespace Solve.Libraries.SegmentTree.SegmentTree
{
    public class SegmentTree<T>
    {
        readonly int _size;
        readonly T[] _seg;
        readonly Func<T, T, T> _func;
        readonly T _identity;

        public SegmentTree(int N, Func<T, T, T> func, T identity = default) {
            _func = func;
            _identity = identity;
            _size = 1 << (int) System.Math.Ceiling(System.Math.Log(N, 2));
            _seg = Enumerable.Repeat(_identity, _size * 2).ToArray();
        }

        public void Set(int i, T value) {
            if (i < 0 || i >= _size) throw new ArgumentOutOfRangeException(nameof(i));
            _seg[_size + i] = value;
        }

        public void Build() {
            for (int i = _size - 1; i > 0; i--) _seg[i] = _func(_seg[2 * i], _seg[2 * i + 1]);
        }

        public void Update(int i, T value) {
            i += _size;
            _seg[i] = value;
            while ((i >>= 1) > 0) _seg[i] = _func(_seg[2 * i], _seg[2 * i + 1]);
        }

        public T Query(int l, int r) {
            T L = _identity, R = _identity;
            for (l += _size, r += _size; l < r; l >>= 1, r >>= 1) {
                if ((l & 1) > 0) L = _func(L, _seg[l++]);
                if ((r & 1) > 0) R = _func(_seg[--r], R);
            }
 
            return _func(L, R);
        }

        public T AllProd() => _seg[1];

        public int FindSubtree(int a, Func<T, bool> check, T M, bool type) {
            int t = type ? 1 : 0;
            while (a < _size) {
                var nxt = type ? _func(_seg[2 * a + t], M) : _func(M, _seg[2 * a + t]);
                if (check(nxt)) a = 2 * a + t;
                else (M, a) = (nxt, 2 * a + 1 - t);
            }

            return a - _size;
        }

        public int FindFirst(int a, Func<T, bool> check) {
            var L = _identity;
            if (a <= 0) {
                if (check(_func(L, _seg[1]))) return FindSubtree(1, check, L, false);
                return -1;
            }

            int b = _size;
            for (a += _size, b += _size; a < b; a >>= 1, b >>= 1) {
                if ((a & 1) != 0) {
                    var nxt = _func(L, _seg[a]);
                    if (check(nxt)) return FindSubtree(a, check, L, false);
                    L = nxt;
                    ++a;
                }
            }

            return -1;
        }

        public int FindLast(int b, Func<T, bool> check) {
            var R = _identity;
            if (b >= _size) {
                if (check(_func(_seg[1], R))) return FindSubtree(1, check, R, true);
                return -1;
            }

            int a = _size;
            for (b += _size; a < b; a >>= 1, b >>= 1) {
                if ((b & 1) != 0) {
                    var nxt = _func(_seg[--b], R);
                    if (check(nxt)) return FindSubtree(b, check, R, true);
                    R = nxt;
                }
            }

            return -1;
        }

        public T this[int i] => _seg[i + _size];
    }
}