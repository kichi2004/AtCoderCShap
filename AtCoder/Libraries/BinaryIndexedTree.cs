using System;
using System.Collections.Generic;

namespace Solve.Libraries.BinaryIndexedTree
{
    public class BinaryIndexedTree
    {
        public BinaryIndexedTree(IReadOnlyList<long> values) : this(values.Count) {
            for (int i = 0; i < values.Count; i++) Add(i, values[i]);
        }
        public BinaryIndexedTree(IReadOnlyList<int> values) : this(values.Count) {
            for (int i = 0; i < values.Count; i++) Add(i, values[i]);
        }
        public BinaryIndexedTree(int N) {
            int count = 1;
            while (count < N) count <<= 1;
            _bit = new long[count + 1];
            _size = N;
        }
        
        readonly long[] _bit;
        readonly int _size;
        
        long Query(in int index) {
            long res = 0;
            for (int i = index; i > 0; i -= i & -i) res += _bit[i];
            return res;
        }
        public long Query(in int start, in int end) => Query(end) - Query(start);

        public long this[in Range range] {
            get {
                int start = range.Start.Value, end = range.End.Value;
                if (range.Start.IsFromEnd) start = _size - start;
                if (range.End.IsFromEnd) end = _size - end;
                return Query(start, end);
            }
        }

        public long Add(in int index, in long value) {
            for (int i = index + 1; i < _bit.Length; i += i & -i) _bit[i] += value;
            return value;
        }
    }
}
