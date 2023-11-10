using System.Numerics;

namespace Solve.Libraries.Math
{
    public static class Math
    {
        public static IEnumerable<T> PrimeFactors<T>(T value)
            where T : INumberBase<T>, IComparisonOperators<T, T, bool>, IModulusOperators<T, T, T> {
            var first = value;
            for (var i = T.CreateChecked(2); i * i <= first; i++) {
                while (T.IsZero(value % i)) {
                    value /= i;
                    yield return i;
                }
            }

            if (value != T.MultiplicativeIdentity)
                yield return value;
        }

        public static IReadOnlyDictionary<T, int> PrimeFactorize<T>(T value)
            where T : INumberBase<T>, IComparisonOperators<T, T, bool>, IModulusOperators<T, T, T> {
            var dict = new Dictionary<T, int>();
            var first = value;
            for (var i = T.CreateChecked(2); i * i <= first; i++) {
                if (T.IsZero(value % i)) continue;
                int cnt = 0;
                while (T.IsZero(value % i)) {
                    value /= i;
                    cnt++;
                }

                dict.Add(i, cnt);
            }

            if (value != T.MultiplicativeIdentity) dict.Add(value, 1);
            return dict;
        }

        public static long DivisorsCount<T>(T value)
            where T : INumberBase<T>, IComparisonOperators<T, T, bool>, IModulusOperators<T, T, T> {
            var fact = PrimeFactorize(value);
            return fact.Select(x => x.Value + 1L).Aggregate((m, x) => m * x);
        }

        public static long Combinations(int n, int r) {
            if (n < r) return 0;
            if (r > n - r) r = n - r;

            long ans = 1;
            for (int i = 0; i < r; ++i) ans *= n - i;
            for (int i = 2; i <= r; ++i) ans /= i;

            return ans;
        }

        public static long Permutations(int n, int r) {
            if (n < r) return 0;
            long ans = 1;
            for (int i = 0; i < r; ++i) ans *= n - i;
            return ans;
        }

        public static T Factorial<T>(T n) where T : INumberBase<T>, IComparisonOperators<T, T, bool> {
            T ans = T.MultiplicativeIdentity;
            for (T i = T.CreateChecked(2); i <= n; ++i) ans *= i;
            return ans;
        }
    }
}