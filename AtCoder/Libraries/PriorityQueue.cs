using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Solve.Libraries.PriorityQueue
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        public bool Any() => Count > 0;
        public int Count { get; set; }
        readonly bool descendance;
        T[] data = new T[65536];
        readonly IComparer<T> comparer;

        public PriorityQueue(bool descendance = false, IComparer<T> comparer = null) =>
            (this.descendance, this.comparer) = (descendance, comparer ?? Comparer<T>.Default);

        ///<remarks>O(1)</remarks>
        public T Top {
            [MethodImpl(256)]
            get {
                Check();
                return data[1];
            }
        }

        /// <remarks>O(log N)</remarks>
        public T Pop() {
            var top = Top;
            var elem = data[Count--];
            int index = 1;
            while (true) {
                if (index << 1 >= Count) {
                    if (index << 1 > Count) break;
                    if (comparer.Compare(elem, data[index << 1]) > 0 ^ descendance) data[index] = data[index <<= 1];
                    else break;
                } else {
                    var nextIndex =
                        comparer.Compare(data[index << 1], data[(index << 1) + 1]) <= 0 ^ descendance
                            ? index << 1
                            : (index << 1) + 1;
                    if (comparer.Compare(elem, data[nextIndex]) > 0 ^ descendance)
                        data[index] = data[index = nextIndex];
                    else break;
                }
            }

            data[index] = elem;
            return top;
        }

        public void Add(T value) => Push(value);

        /// <remarks>O(log N)</remarks>
        public void Push(T value) {
            int index = ++Count;
            if (data.Length == Count) Extend(data.Length * 2);
            while (index >> 1 != 0) {
                if (comparer.Compare(data[index >> 1], value) > 0 ^ descendance)
                    data[index] = data[index >>= 1];
                else break;
            }

            data[index] = value;
        }

        void Extend(int newSize) {
            var newDatas = new T[newSize];
            data.CopyTo(newDatas, 0);
            data = newDatas;
        }

        bool Check() => Count > 0 ? true : throw new Exception();
    }
}