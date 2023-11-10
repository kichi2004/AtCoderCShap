using System.Runtime.CompilerServices;

namespace Solve.Libraries.PriorityQueue
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        public bool Any() => Count > 0;
        public int Count { get; set; }
        readonly bool _descendance;
        T[] _data = new T[65536];
        readonly IComparer<T> _comparer;

        public PriorityQueue(bool descendance = false, IComparer<T> comparer = null) =>
            (this._descendance, this._comparer) = (descendance, comparer ?? Comparer<T>.Default);

        ///<remarks>O(1)</remarks>
        public T Top {
            [MethodImpl(256)]
            get {
                Check();
                return _data[1];
            }
        }

        /// <remarks>O(log N)</remarks>
        public T Pop() {
            var top = Top;
            var elem = _data[Count--];
            int index = 1;
            while (true) {
                if (index << 1 >= Count) {
                    if (index << 1 > Count) break;
                    if (_comparer.Compare(elem, _data[index << 1]) > 0 ^ _descendance) _data[index] = _data[index <<= 1];
                    else break;
                } else {
                    var nextIndex =
                        _comparer.Compare(_data[index << 1], _data[(index << 1) + 1]) <= 0 ^ _descendance
                            ? index << 1
                            : (index << 1) + 1;
                    if (_comparer.Compare(elem, _data[nextIndex]) > 0 ^ _descendance)
                        _data[index] = _data[index = nextIndex];
                    else break;
                }
            }

            _data[index] = elem;
            return top;
        }

        public void Add(T value) => Push(value);

        /// <remarks>O(log N)</remarks>
        public void Push(T value) {
            int index = ++Count;
            if (_data.Length == Count) Extend(_data.Length * 2);
            while (index >> 1 != 0) {
                if (_comparer.Compare(_data[index >> 1], value) > 0 ^ _descendance)
                    _data[index] = _data[index >>= 1];
                else break;
            }

            _data[index] = value;
        }

        void Extend(int newSize) {
            var newDatas = new T[newSize];
            _data.CopyTo(newDatas, 0);
            _data = newDatas;
        }

        bool Check() => Count > 0 ? true : throw new Exception();
    }
}