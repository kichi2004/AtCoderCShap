using System.Numerics;

namespace Solve.Libraries.BinaryIndexedTree
{
    public class BinaryIndexedTree<T> where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        public BinaryIndexedTree(IReadOnlyList<T> values) : this(values.Count) {
            for (int i = 0; i < values.Count; i++) Add(i, values[i]);
        }
        public BinaryIndexedTree(int N) {
            int count = 1;
            while (count < N) count <<= 1;
            _bit = new T[count + 1];
            _size = N;
        }
        
        readonly T[] _bit;
        readonly int _size;
        
        T Query(in int index) {
            var res = T.AdditiveIdentity;
            for (int i = index; i > 0; i -= i & -i) res += _bit[i];
            return res;
        }
        public T Query(in int start, in int end) => Query(end) - Query(start);

        public T this[in Range range] {
            get {
                int start = range.Start.Value, end = range.End.Value;
                if (range.Start.IsFromEnd) start = _size - start;
                if (range.End.IsFromEnd) end = _size - end;
                return Query(start, end);
            }
        }

        // public T Add(in Index index, in T value) => Add(index.IsFromEnd ? _size - index.Value : index.Value, value);
        public T Add(in int index, in T value) {
            for (int i = index + 1; i < _bit.Length; i += i & -i) _bit[i] += value;
            return value;
        }
    
        public static BinaryIndexedTree<T> Create(IReadOnlyList<T> values) => new BinaryIndexedTree<T>(values);
    }
}
