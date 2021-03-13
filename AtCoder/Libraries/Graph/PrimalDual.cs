using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Solve.Libraries.PriorityQueue;

namespace Solve.Libraries.Graph.PrimalDual
{
    using TCost = Int64;
    using TFlow = Int64;

    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public class PrimalDual
    {
        private readonly long INF;

        public class Edge
        {
            public Edge(int to, long capacity, long cost, int rev, bool isRev) =>
                (To, Capacity, Cost, Rev, IsRev) = (to, capacity, cost, rev, isRev);

            public Edge(int from, int to, long flow, long capacity) =>
                (From, To, Flow, Capacity) = (from, to, flow, capacity);
            
            public readonly int From, To;
            public long Capacity;
            public readonly long Flow;
            internal readonly long Cost;
            internal readonly int Rev;
            internal readonly bool IsRev;
        }

        readonly List<Edge>[] graph;

        public IEnumerable<Edge> Edges
        {
            get
            {
                var ret = new List<Edge>();
                for (int i = 0; i < graph.Length; i++)
                    ret.AddRange(
                        graph[i]
                            .Where(e => !e.IsRev)
                            .Select(e => new {e, rev_e = graph[e.To][e.Rev]})
                            .Select(t => new Edge(i, t.e.To, t.rev_e.Capacity, t.rev_e.Capacity + t.e.Capacity))
                    );

                return ret;
            }
        }

        public PrimalDual(int V)
        {
            graph = new List<Edge>[V];
            for (int i = 0; i < V; i++) graph[i] = new List<Edge>();
            INF = long.MaxValue;
        }

        public void AddEdge(int from, int to, long cap, long cost)
        {
            graph[from].Add(new Edge(to, cap, cost, graph[to].Count, false));
            graph[to].Add(new Edge(from, 0, -cost, graph[from].Count - 1, true));
        }

        public long? MinCostFlow(int s, int t, long f)
        {
            int V = graph.Length;
            long ret = 0;
            var que = new PriorityQueue<(long, int)>();
            var potential = new long[V];
            Array.Fill(potential, 0);
            var minCost = new long[V];
            var preve = new int[V];
            Array.Fill(preve, -1);
            var prevv = new int[V];
            Array.Fill(prevv, -1);

            while (f > 0)
            {
                Array.Fill(minCost, INF);
                que.Add((0, s));
                minCost[s] = 0;
                while (que.Any())
                {
                    (long cost, int index) = que.Pop();
                    if (minCost[index] < cost) continue;
                    for (int i = 0; i < graph[index].Count; i++)
                    {
                        var e = graph[index][i];
                        long nextCost = minCost[index] + e.Cost + potential[index] - potential[e.To];
                        if (e.Capacity > 0 && minCost[e.To] > nextCost)
                        {
                            minCost[e.To] = nextCost;
                            prevv[e.To] = index;
                            preve[e.To] = i;
                            que.Add((minCost[e.To], e.To));
                        }
                    }
                }

                if (minCost[t] == INF) return null;
                for (int v = 0; v < V; v++) potential[v] += minCost[v];
                long addflow = f;
                for (int v = t; v != s; v = prevv[v])
                    addflow = System.Math.Min(addflow, graph[prevv[v]][preve[v]].Capacity);

                f -= addflow;
                ret += (long)addflow * potential[t];
                for (int v = t; v != s; v = prevv[v])
                {
                    var e = graph[prevv[v]][preve[v]];
                    e.Capacity -= addflow;
                    graph[v][e.Rev].Capacity += addflow;
                }
            }

            return ret;
        }

        public void Output()
        {
            for (int i = 0; i < graph.Length; i++)
                foreach (var e in graph[i])
                {
                    if (e.IsRev) continue;
                    var rev_e = graph[e.To][e.Rev];
                    Console.WriteLine($"{i} -> {e.To} (flow: {rev_e.Capacity} / {rev_e.Capacity + e.Capacity})");
                }
        }
    }
}
