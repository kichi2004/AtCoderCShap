using System.Collections.Generic;
using System.Linq;

namespace Solve.Libraries.Math
{
    public static class Math
    {
        public static IEnumerable<int> PrimeFactors(int value) {
            int first = value;
            for (int i = 2; i * i <= first; i++) {
                while (value % i == 0) {
                    value /= i;
                    yield return i;
                }
            }

            if (value > 1)
                yield return value;
        }

        public static IEnumerable<long> PrimeFactors(long value) {
            long first = value;
            for (long i = 2; i * i <= first; i++) {
                while (value % i == 0) {
                    value /= i;
                    yield return i;
                }
            }

            if (value > 1)
                yield return value;
        }

        public static IReadOnlyDictionary<long, int> PrimeFactorize(long value) {
            var dict = new Dictionary<long, int>();
            long first = value;
            for (int i = 2; i * i <= first; i++) {
                if (value % i > 0) continue;
                int cnt = 0;
                while (value % i == 0) {
                    value /= i;
                    cnt++;
                }

                dict.Add(i, cnt);
            }

            if (value > 1) dict.Add(value, 1);
            return dict;
        }

        public static IReadOnlyDictionary<int, int> PrimeFactorize(int value) {
            var dict = new Dictionary<int, int>();
            int first = value;
            for (int i = 2; i * i <= first; i++) {
                if (value % i > 0) continue;
                int cnt = 0;
                while (value % i == 0) {
                    value /= i;
                    cnt++;
                }

                dict.Add(i, cnt);
            }

            if (value > 1) dict.Add(value, 1);
            return dict;
        }

        public static long DivisorsCount(long value) {
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

        public static long Factorial(int n) {
            long ans = 1;
            for (int i = 2; i <= n; ++i) ans *= i;
            return ans;
        }
    }
}