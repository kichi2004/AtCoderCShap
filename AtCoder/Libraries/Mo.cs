using System.Collections.Generic;
using System.Linq;

namespace Solve.Libraries.Mo
{
    public abstract class Mo
    {
        private List<(int left, int right)> Queries { get; }

        int Width { get; }

        public Mo(int width) {
            Width = width;
            Queries = new List<(int left, int right)>();
        }
        
        /// <summary>区間を追加します．（開区間）</summary>
        public void AddQuery(int left, int right) => Queries.Add((left, right));

        protected abstract void Add(int index);
        protected void AddLeft(int index) => Add(index);
        protected void AddRight(int index) => Add(index);
        
        protected abstract void Erase(int index);
        protected void EraseLeft(int index) => Erase(index);
        protected void EraseRight(int index) => Erase(index);
        
        protected abstract void Answer(int index);
        
        public void Run() {
            var queries = Queries.Select((query, i) => new {
                query.left,
                query.right,
                index = i,
            }).ToList();
            queries.Sort((s, t) => {
                int slb = s.left / Width, tlb = t.left / Width;
                int srb = s.right / Width, trb = t.right / Width;

                if (slb != tlb) {
                    return slb.CompareTo(tlb);
                }
                if (slb % 2 == 1) return trb.CompareTo(srb);
                return srb.CompareTo(trb);
            });
            
            
            var l = 0;
            var r = 0;
            foreach (var query in queries) {
                int ql = query.left, qr = query.right;
                while (l > ql) AddLeft(--l);
                while (r < qr) AddRight(r++);
                while (l < ql) EraseLeft(l++);
                while (r > qr) EraseRight(--r);
                Answer(query.index);
            }
        }
    }
}
