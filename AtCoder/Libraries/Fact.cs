namespace Solve.Libraries.Fact
{
    public class Fact
    {
        //i!
        readonly int[] _fact;

        //i!^-1
        readonly int[] _inv;

        public Fact() : this(200010) { }

        public Fact(int N)
        {
            var lastFact = ModInt.ModInt.One;
            _fact = new int[N + 1];
            _fact[0] = 1;
            Lib.RepeatClosed(N, i => {
                lastFact *= i;
                _fact[i] = lastFact.Value;
            });

            var lastInv = lastFact.Invert;
            _inv = new int[N + 1];
            _inv[N] = lastInv.Value;
            Lib.RepeatReverse(N, i => {
                lastInv *= i + 1;
                _inv[i] = lastInv.Value;
            });

            _inv[0] = _inv[1];
        }

        public int this[int i] => _fact[i];
        public int Inv(int i) => _inv[i];

        public int nCr(int n, int r) {
            if (n - r < 0) return 0;
            if (n >= _fact.Length)
                throw new ArgumentOutOfRangeException(nameof(n));
            var ret = ModInt.ModInt.One;
            // n! / (n-r)!r!
            ret *= _fact[n];
            ret *= _inv[n - r];
            ret *= _inv[r];
            return ret.Value;
        }

        public int nHr(int n, int r) {
            n = r + n - 1;
            if (n - r < 0) return 0;
            if (n >= _fact.Length)
                throw new ArgumentOutOfRangeException(nameof(n));
            return nCr(n, r);
        }

        public int nPr(int n, int r) {
            if (n - r < 0) return 0;
            if (n >= _fact.Length)
                throw new ArgumentOutOfRangeException(nameof(n));
            var ret = ModInt.ModInt.One;
            // n! / (n-r)!r!
            ret *= _fact[n];
            ret *= _inv[n - r];
            return ret.Value;
        }
    }
}