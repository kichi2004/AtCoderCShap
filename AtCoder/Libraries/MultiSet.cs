using System;
using Solve.Libraries.BinarySearchTreeUtils;
using Solve.Libraries.Set;

namespace Solve.Libraries.MultiSet {
    public class MultiSet<T> : Set<T> where T : IComparable {
        public override void Add(T v) => 
            _root = _root == null ? new BinarySearchTree<T>.Node(v) : BinarySearchTree<T>.Insert(_root, v);
    }
}