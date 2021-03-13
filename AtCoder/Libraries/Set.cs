using System;
using System.Collections;
using System.Collections.Generic;
using Solve.Libraries.BinarySearchTreeUtils;

namespace Solve.Libraries.Set {
    public class Set<T> : IEnumerable<T> where T : IComparable {
        
        protected BinarySearchTree<T>.Node _root;

        public T this[int idx] => ElementAt(idx);

        public int Count => BinarySearchTree<T>.Count(_root);

        public virtual void Add(T v)
        {
            if (_root == null) _root = new BinarySearchTree<T>.Node(v);
            else
            {
                if (BinarySearchTree<T>.Find(_root, v) != null) return;
                _root = BinarySearchTree<T>.Insert(_root, v);
            }
        }

        public void Clear() => _root = null;

        public void Remove(T v) => _root = BinarySearchTree<T>.Remove(_root, v);

        public bool Contains(T v) => BinarySearchTree<T>.Contains(_root, v);

        public T ElementAt(int k)
        {
            var node = BinarySearchTree<T>.FindByIndex(_root, k);
            if (node == null) throw new IndexOutOfRangeException();
            return node.Value;
        }

        public int CountOf(T v) => BinarySearchTree<T>.UpperBound(_root, v) - BinarySearchTree<T>.LowerBound(_root, v);

        public int LowerBound(T v) => BinarySearchTree<T>.LowerBound(_root, v);

        public int UpperBound(T v) => BinarySearchTree<T>.UpperBound(_root, v);

        public (int, int) EqualRange(T v) =>
            !Contains(v)
                ? (-1, -1)
                : (BinarySearchTree<T>.LowerBound(_root, v), BinarySearchTree<T>.UpperBound(_root, v) - 1);


        public IEnumerator<T> GetEnumerator() => BinarySearchTree<T>.Enumerate(_root).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
