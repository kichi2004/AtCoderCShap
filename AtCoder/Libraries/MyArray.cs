using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.Libraries.MyArray
{
    public class Sequence<T> : IEnumerable<T>, IList<T>
    {
        T[] _array = null;
        int _indexBase = 0;
        public IEnumerator<T> GetEnumerator() => _array.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();
        public void Add(T item) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(T item) => _array.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => array.CopyTo(array, arrayIndex);
        public bool Remove(T item) => throw new NotSupportedException();
        public int Count => _array.Length;
        public bool IsReadOnly { get; } = false;
        public int IndexOf(T item) => Array.IndexOf(_array, item);
        public void Insert(int index, T item) => throw new NotSupportedException();
        public void RemoveAt(int index) => throw new NotSupportedException();
        public T this[int index]
        {
            get => _array[index + _indexBase];
            set => _array[index + _indexBase] = value;
        }
    }
}
