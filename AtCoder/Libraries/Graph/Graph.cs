using System.Collections.Generic;
using static Solve.Methods;

namespace Solve.Libraries.Graph.Graph
{
    public interface IEdge
    {
        int To { get; } 
        long Cost { get; }
        public void Deconstruct(out int t, out long c) => (t, c) =(To, Cost);
    }
    public interface Graph<EdgeType> where EdgeType : IEdge
    {
        int Length { get; }
        List<EdgeType> this[int i] { get; }
        List<EdgeType>[] Graph { get; }
        void InputGraph(int m, in bool directed, in int offset);
    }
    public readonly struct WeightEdge : IEdge
    {
        public WeightEdge(int to, long cost) => (To, Cost) = (to, cost);
        public int To { get; }
        public long Cost { get; }
        public void Deconstruct(out int t, out long c) => (t, c) = (To, Cost);
    }
    public class WeightGraph : Graph<WeightEdge>
    {
        public WeightGraph(int n)
        {
            Graph = new List<WeightEdge>[n]; 
            rep(n, i => Graph[i] = new List<WeightEdge>());
        }
        public int Length => Graph.Length;
        public List<WeightEdge> this[int i] => Graph[i];
        public List<WeightEdge>[] Graph { get; }
        public void InputGraph(int m, in bool directed = true, in int offset = 1)
        {
            while (m-- > 0)
            {
                var (a, b, c) = Input.next<int, int, long>();
                a -= offset;
                b -= offset;
                Graph[a].Add(new WeightEdge(b, c));
                if (!directed) Graph[b].Add(new WeightEdge(a, c));
            }
        }
        public void AddEdge(in int from, in int to, in long cost, in bool directed = true) 
        {
            Graph[from].Add(new WeightEdge(to, cost));
            if(!directed) Graph[to].Add(new WeightEdge(from, cost));
        }
    }

    public readonly struct UnweightEdge : IEdge
    {
        public UnweightEdge(int to) => To = to;
        public int To { get; }
        public long Cost => 1;
        public void Deconstruct(out int t, out long c) => (t, c) = (To, Cost);
    }

    public class UnweightGraph : Graph<UnweightEdge>
    {
        public UnweightGraph(int n)
        {
            Graph = new List<UnweightEdge>[n]; 
            rep(n, i => Graph[i] = new List<UnweightEdge>());
        }
        public int Length => Graph.Length;
        public List<UnweightEdge> this[int i] => Graph[i];
        public List<UnweightEdge>[] Graph { get; }
        public void InputGraph(int m, in bool directed = true, in int offset = 1)
        {
            while (m-- > 0)
            {
                var (a, b) = Input.next<int, int>();
                a -= offset;
                b -= offset;
                Graph[a].Add(new UnweightEdge(b));
                if (!directed) Graph[b].Add(new UnweightEdge(a));
            }
        }
    }
}
