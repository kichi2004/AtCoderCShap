using System;
using System.Collections.Generic;
using System.Linq;
using Solve.Libraries.Graph.Graph;
using static Solve.Lib;

namespace Solve.Libraries.Graph.BellmanFord
{
    public class BellmanFord
    {
        public const long INF = 1000000000000000100;
        readonly int _n;
        readonly List<IEdge>[] _graph;

        public BellmanFord(int N)
        {
            _graph = InitArray(N, _ => new List<IEdge>());
            _n = N;
        }

        public BellmanFord(WeightGraph graph) : this(graph.Length) {
            for (int i = 0; i < _n; i++) _graph[i].AddRange(graph[i].Cast<IEdge>());
        }
        public BellmanFord(UnweightGraph graph) : this(graph.Length)
        {
            for (int i = 0; i < _n; i++) _graph[i].AddRange(graph[i].Cast<IEdge>());
        }

        public void Add(int start, int end, long cost, bool undirected = false)
        {
            Validate(start, nameof(start)); Validate(end, nameof(end));
            _graph[start].Add(new WeightEdge(end, cost));
            if (undirected) _graph[end].Add(new WeightEdge(start, cost));
        }

        IEnumerable<(int from, int to, long cost)> Edges {
            get { for (int i = 0; i < _n; i++) foreach (var (to, cost) in _graph[i]) yield return (i, to, cost); }
        }

        /// <remarks>O(VE)</remarks>
        /// <returns>Shortest pathes from start, null if contains negative cycle.</returns>
        public long[] Run(int start, long firstCost = 0)
        {
            Validate(start, nameof(start));

            var dist = InitArray(_n, _ => INF);
            dist[start] = firstCost;
            for (int i = 0; i < _n; i++) {
                foreach (var (from, to, cost) in Edges) {
                    if (dist[from] == INF) continue;
                    if (chmin(ref dist[to], dist[from] + cost) && i == _n - 1) return null;
                }
            }

            return dist;
        }

        void Validate(int val, string argument) {
            if (val.IsIn(.._n)) return;
            throw new ArgumentOutOfRangeException(argument);
        }
    }
}
