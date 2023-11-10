using Solve.Libraries.Graph.Graph;
using Solve.Libraries.PriorityQueue;
using static Solve.Lib;

namespace Solve.Libraries.Graph.Dijkstra
{
    public class Dijkstra
    {
        public struct Node : IComparable<Node>
        {
            public Node(int num, int from, long cost) => (Number, From, Cost) = (num, from, cost);
            public readonly int Number;
            public int From { get; internal set; }
            public long Cost { get; internal set; }
            public bool IsReachable => Cost != INF;

            internal Node Copy() => new Node(Number, From, Cost);
            public static bool operator >(Node a, Node b) => a.Cost > b.Cost;
            public static bool operator <(Node a, Node b) => a.Cost < b.Cost;
            public int CompareTo(Node other) => Cost.CompareTo(other.Cost);
        }

        public const long INF = 1000000000000000100;
        readonly int _n;
        readonly List<IEdge>[] _graph;

        public Dijkstra(int N)
        {
            _graph = InitArray(N, _ => new List<IEdge>());
            _n = N;
        }

        public Dijkstra(WeightGraph graph) : this(graph.Length) {
            for (int i = 0; i < _n; i++) _graph[i].AddRange(graph[i].Cast<IEdge>());
        }
        public Dijkstra(UnweightGraph graph) : this(graph.Length)
        {
            for (int i = 0; i < _n; i++) _graph[i].AddRange(graph[i].Cast<IEdge>());
        }

        public void Add(int start, int end, long cost, bool undirected = false)
        {
            Validate(start, nameof(start)); Validate(end, nameof(end));
            _graph[start].Add(new WeightEdge(end, cost));
            if (undirected) _graph[end].Add(new WeightEdge(start, cost));
        }

        /// <remarks>O((E+V) log V)</remarks>
        public Node[] Run(int start, long firstCost = 0)
        {
            Validate(start, nameof(start));
            var que = new PriorityQueue<Node>();
            var node = InitArray(_n, i => new Node(i, -1, INF));
            node[start].Cost = firstCost;

            foreach (var (to, cost) in _graph[start])
            {
                node[to].Cost = cost + node[start].Cost;
                node[to].From = start;
                que.Push(node[to]);
            }

            while (que.Any())
            {
                var next = que.Pop();
                if (next.Cost > node[next.Number].Cost) continue;
                foreach (var (to, cost) in _graph[next.Number].Where(e => node[e.To].Cost > next.Cost + e.Cost)) {
                    node[to].Cost = next.Cost + cost;
                    node[to].From = next.Number;
                    que.Push(node[to]);
                }
            }

            return node;
        }

        public static Node[] Restore(IReadOnlyList<Node> nodes, int to)
        {
            var node = nodes[to];
            var list = new List<Node> {node};
            while (node.Number != node.From) list.Add(node = nodes[node.From]);
            list.Reverse();
            return list.ToArray();
        }

        void Validate(int val, string argument) {
            if (val.IsIn(.._n)) return;
            throw new ArgumentOutOfRangeException(argument);
        }
    }
}