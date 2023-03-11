using System;

namespace Solve.Libraries.CumulativeSum2D {
    [Obsolete]
    public class CumulativeSum2D {
        public CumulativeSum2D(long[][] array) : this(array.To2DArray()) { }

        public CumulativeSum2D(long[,] array) {
            var n = array.GetLength(0);
            var m = array.GetLength(1);

            _cumSum = new long[n + 1, m + 1];
            for(int i = 1; i <= n; ++i)
            for (int j = 1; j <= m; ++j) {
                _cumSum[i, j] = array[n - 1, m - 1] + _cumSum[i - 1, j] + _cumSum[i, j - 1] - _cumSum[i - 1, j - 1];
            }
        }
        readonly long[,] _cumSum;

        
        public long GetSum((int i, int j) x, (int i, int j) y) {
            var (i1, j1) = x;
            var (i2, j2) = y;
            Lib.Assert(i1 <= i2);
            Lib.Assert(j1 <= j2);

            return _cumSum[i2, j2] - _cumSum[i1, j2] - _cumSum[i2, j1] + _cumSum[i1, j1];
        }
    }
}
