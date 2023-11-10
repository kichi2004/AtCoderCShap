// ReSharper disable RedundantUsingDirective
#nullable disable

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using static Solve.Input;
using static Solve.Lib;
using static Solve.Output;
using static System.Math;
using Range = System.Range;

// Resharper restore RedundantUsingDirective
using System;

namespace Solve
{
    public record In;

    public partial class Solver
    {
        public void Main(In cin)
        {
            
        }

        public const long MOD = 998244353;
    }


    public static class Lib
    {
        [MethodImpl(256)] public static bool Assert(in bool b, in string message = null) =>
            b ? true : throw new Exception(message ?? "Assert failed.");
        [MethodImpl(256)] public static string JoinSpace<T>(this IEnumerable<T> source) => source.Join(" ");
        [MethodImpl(256)] public static string JoinEndline<T>(this IEnumerable<T> source) => source.Join("\n");
        [MethodImpl(256)] public static string Join<T>(this IEnumerable<T> source, string s) => string.Join(s, source);
        [MethodImpl(256)] public static string Join<T>(this IEnumerable<T> source, char c) => string.Join(c.ToString(), source);
        public static T Gcd<T>(T a, T b) where T : INumber<T> {
            while (true) {
                if (a < b) (a, b) = (b, a);
                if (T.IsZero(a % b)) return b;
                (a, b) = (b, a % b);
            }
        }
        public static T Lcm<T>(T a, T b) where T : INumber<T> => a / Gcd(a, b) * b;
        public static bool IsPrime(long value) {
            if (value <= 1) return false;
            for (long i = 2; i * i <= value; ++i) if (value % i == 0) return false;
            return true;
        }
        public static long Pow(long a,  int b) {
            long res = 1;
            while (b > 0) { if (b % 2 != 0) res *= a; a *= a; b >>= 1; }
            return res;
        }
        public static int PowMod(long a, long b, int p) => (int) PowMod(a, b, (long) p);
        public static long PowMod(long a, long b, long p) {
            long res = 1;
            while (b > 0) { if (b % 2 != 0) res = res * a % p; a = a * a % p; b >>= 1; }
            return res;
        }
        public static IEnumerable<T> Factors<T>(T n) where T : INumber<T> {
            Assert(T.IsPositive(n), "n must be greater than 0.");
            for (var i = T.One; i * i <= n; ++i)
            {
                if (!T.IsZero(n % i)) continue;
                var div = n / i;
                yield return div;
                if (i != div) yield return i;
            }
        }
        [MethodImpl(256)] public static T DivCeil<T>(T a, T b) where T : INumber<T> => (a + b - T.One) / b;
        public static IEnumerable<T[]> Permutations<T>(IEnumerable<T> src) {
            var ret = new List<T[]>();
            Search(ret, new Stack<T>(), src.ToArray());
            return ret;

            static void Search(ICollection<T[]> perms, Stack<T> stack, T[] a) {
                int N = a.Length;
                if (N == 0) perms.Add(stack.Reverse().ToArray());
                else {
                    var b = new T[N - 1];
                    Array.Copy(a, 1, b, 0, N - 1);
                    for (int i = 0; i < a.Length; ++i) {
                        stack.Push(a[i]);
                        Search(perms, stack, b);
                        if (i < b.Length) b[i] = a[i];
                        stack.Pop();
                    }
                }
            }
        }
        public static IEnumerable<bool[]> AllSubsets(int n) {
            Assert(n < 31, "n must be less than 31.");
            var range = Range(n).ToArray();
            for (int s = 0; s < 1 << n; s++) yield return Array.ConvertAll(range, i => (s & (1 << i)) > 0);
        }
        public static IEnumerable<T[]> AllSubsets<T>(T[] array) {
            foreach (bool[] subset in AllSubsets(array.Length))
                yield return subset.Select((x, i) => (f: x, i)).Where(x => x.f).Select(x => array[x.i]).ToArray();
        }
        public static long BinarySearch(long low, long high, Func<long, bool> expression) {
            while (low < high) {
                long middle = (high - low) / 2 + low;
                if (!expression(middle))
                    high = middle;
                else
                    low = middle + 1;
            }

            return high;
        }
        public static int LowerBound<T>(T[] arr, int start, int end, T value, IComparer<T> comparer) {
            int low = start;
            int high = end;
            while (low < high) {
                var mid = ((high - low) >> 1) + low;
                if (comparer.Compare(arr[mid], value) < 0)
                    low = mid + 1;
                else
                    high = mid;
            }

            return low;
        }
        public static int LowerBound<T>(T[] arr, T value) where T : IComparable =>
            LowerBound(arr, 0, arr.Length, value, Comparer<T>.Default);
        public static int UpperBound<T>(T[] arr, int start, int end, T value, IComparer<T> comparer) {
            var (low, high) = (start, end);
            while (low < high) {
                var mid = ((high - low) >> 1) + low;
                if (comparer.Compare(arr[mid], value) <= 0) low = mid + 1;
                else high = mid;
            }

            return low;
        }
        public static int UpperBound<T>(T[] arr, T value) => 
            UpperBound(arr, 0, arr.Length, value, Comparer<T>.Default);
        [MethodImpl(256)]
        public static IEnumerable<TResult> Repeat<TResult>(TResult value, int count) => Enumerable.Repeat(value, count);
        [MethodImpl(256)] public static string AsString(this IEnumerable<char> source) => new string(source.ToArray());
        public static IEnumerable<T> CumulativeSum<T>(this IEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity; foreach (var item in source) yield return sum += item;
        }
        [MethodImpl(256)] public static bool IsIn<T>(this T value, T l, T r) where T : IComparable<T> =>
            l.CompareTo(r) > 0 ? throw new ArgumentException() : l.CompareTo(value) <= 0 && value.CompareTo(r) < 0;
        [MethodImpl(256)] public static bool IsIn(this in int value, in Range range) =>
            value.IsIn(range.Start.Value, range.End.Value);
        [MethodImpl(256)] public static bool IsIn(this in Index value, in Range range) =>
            value.IsFromEnd && value.Value.IsIn(range.Start.Value, range.End.Value);
        public static IEnumerable<int> Range(int start, int end, int step = 1) {
            for (var i = start; i < end; i += step) yield return i;
        }
        public static IEnumerable<int> Range(int end) => Range(0, end);
        public static IEnumerable<int> Range(Range range, int step = 1) =>
            Range(range.Start.Value, range.End.Value, step);
        [MethodImpl(256)] public static T[] Sorted<T>(this T[] arr) where T : IComparable<T> {
            var array = arr[..]; Array.Sort(array); return array;
        }
        [MethodImpl(256)]
        public static T[] Sorted<T, U>(this T[] arr, Func<T, U> selector) where U : IComparable<U> {
            var array = arr[..];
            Array.Sort(array, (a, b) => selector(a).CompareTo(selector(b)));
            return array;
        }
        [MethodImpl(256)] public static T[] SortedDescending<T>(this T[] arr) where T : IComparable<T> {
            var array = arr[..];
            Array.Sort(array, (a, b) => b.CompareTo(a));
            return array;
        }
        [MethodImpl(256)]
        public static T[] SortedDescending<T, U>(this T[] arr, Func<T, U> selector) where U : IComparable<U> {
            var array = arr[..];
            Array.Sort(array, (a, b) => selector(b).CompareTo(selector(a)));
            return array;
        }
        public static T[] Unique<T>(this IEnumerable<T> arr) {
            var source = arr.ToArray();
            var ret = new List<T>(source.Length);
            var set = new SortedSet<T>();
            ret.AddRange(source.Where(val => set.Add(val)));

            return ret.ToArray();
        }
        [MethodImpl(256)] public static bool chmin<T>(ref T a, T b)
            where T : IComparable<T> {
            if (a.CompareTo(b) > 0) { a = b; return true; }
            return false;
        } 
        [MethodImpl(256)] public static bool chmax<T>(ref T a, T b)
            where T : IComparable<T> {
            if (a.CompareTo(b) < 0) { a = b; return true; }
            return false;
        }
        [MethodImpl(256)] public static bool ChangeToMin<T>(this ref T a, T b)
            where T : struct, IComparable<T> =>
            chmin(ref a, b);
        [MethodImpl(256)] public static bool ChangeToMax<T>(this ref T a, T b)
            where T : struct, IComparable<T> =>
            chmax(ref a, b);

        public static T[] InitArray<T>(int n, Func<int, T> init) {
            var res = new T[n];
            for (int i = 0; i < n; i++) res[i] = init(i);
            return res;
        }
        public static T[] InitArray<T>(int n, Func<T> init) => InitArray(n, _ => init());
        public static T[] InitArray<T>(int n, T t) where T : struct => InitArray(n, _ => t);
        public static T[][] JaggedArray2D<T>(int a, int b, T defaultValue = default) {
            var ret = new T[a][];
            for (int i = 0; i < a; ++i) ret[i] = Enumerable.Repeat(defaultValue, b).ToArray();
            return ret;
        }
        public static T[,] Array2D<T>(int h, int w, T defaultValue = default) {
            var ret = new T[h, w];
            for (int i = 0; i < h; ++i) for (int j = 0; j < w; ++j) ret[i, j] = defaultValue;
            return ret;
        }

        public static T[,] Array2D<T>(int h, int w, Func<int, int, T> factory) {
            var ret = new T[h, w];
            for (int i = 0; i < h; ++i) for (int j = 0; j < w; ++j) ret[i, j] = factory(i, j);
            return ret;
        }
        
        public static T[,] To2DArray<T>(this T[][] array) {
            if (!array.Any()) return new T[0, 0];

            int len = array[0].Length;
            if (array.Any(x => x.Length != len))
                throw new ArgumentException("Length of each array must be same.", nameof(array));

            var ret = new T[array.Length, len];
            for (int i = 0; i < array.Length; ++i) for (int j = 0; j < len; ++j) ret[i, j] = array[i][j];
            return ret;
        }
        [MethodImpl(256)] public static T Min<T>(params T[] col) => col.Min();
        [MethodImpl(256)] public static T Max<T>(params T[] col) => col.Max();
        [MethodImpl(256)] public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> source) =>
            source.Select((x, i) => (x, i));
        [MethodImpl(256)] public static (T, U, V)[] Zip<T, U, V>(
            IReadOnlyCollection<T> t,
            IReadOnlyCollection<U> u,
            IReadOnlyCollection<V> v
        ) {
            Assert(t.Count == u.Count && u.Count == v.Count);
            return t.Zip(u, (a, b) => (a, b))
                .Zip(v, (tuple, c) => (tuple.a, tuple.b, c)).ToArray();
        }
        [MethodImpl(256)] public static void Repeat(in int start, in int end, Action<int> func) {
            for (int i = start; i < end; ++i) func(i);
        }
        [MethodImpl(256)] public static void Repeat(in int end, Action<int> func) => Repeat(0, end, func);
        [MethodImpl(256)] public static void RepeatClosed(in int end, Action<int> func) => Repeat(1, end + 1, func);
        [MethodImpl(256)] public static void RepeatReverse(in int end, Action<int> func) {
            for (int i = end - 1; i >= 0; --i) func(i);
        }
        [MethodImpl(256)]
        public static void Each<T>(this IEnumerable<T> source, Action<T> func) {
            foreach (var item in source) func(item);
        }
        [MethodImpl(256)]
        public static void EachWithIndex<T>(this IEnumerable<T> source, Action<T, int> func) {
            int index = 0; foreach (var item in source) func(item, index++);
        }
        
        public static IEnumerable<U> Scan<T, U>(this IEnumerable<T> source, U seed, Func<U, T, U> func) {
            var tmp = seed;
            foreach (var v in source) yield return func(tmp, v);
        }
        
        [MethodImpl(256)] public static int Bit(in int x) => 1 << x;
        [MethodImpl(256)] public static long BitLong(in int x) => 1L << x;

        public static (T, U)[] Zip<T, U>(this (T[], U[]) arrays) => arrays.Item1.Zip(arrays.Item2).ToArray();

        public static U Pipe<T, U>(this T self, Func<T, U> func) => func(self);
        public static void Pipe<T>(this T self, Action<T> func) => func(self);
        
    }
    public class UnorderedMap<T, U> : Dictionary<T, U>
    {
        public new U this[T k] {
            get =>
                TryGetValue(k, out var v) ? v : base[k] = default;
            set =>
                base[k] = value;
        }
    }
    public class Map<T, U> : SortedDictionary<T, U>
    {
        readonly U _default;
        public Map(U defaultValue = default) {
            _default = defaultValue;
        }
        public new U this[T k] {
            get =>
                TryGetValue(k, out var v) ? v : base[k] = _default;
            set =>
                base[k] = value;
        }
    }
    public class Scanner<T> {
        public Scanner() {
            _deconstructor = new Deconstructor(this);
        }
        public readonly struct Deconstructor {
            public Deconstructor(Scanner<T> scanner) => _sc = scanner;
            
            readonly Scanner<T> _sc;

            public void Deconstruct(out T _1, out T _2) => (_1, _2) = (_sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3) => (_1, _2, _3) = (_sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4) =>
                (_1, _2, _3, _4) = (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4, out T _5) =>
                (_1, _2, _3, _4, _5) = (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4, out T _5, out T _6) =>
                (_1, _2, _3, _4, _5, _6) = (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4, out T _5, out T _6, out T _7) =>
                (_1, _2, _3, _4, _5, _6, _7) =
                (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4, out T _5, out T _6, out T _7, out T _8) =>
                (_1, _2, _3, _4, _5, _6, _7, _8) = 
                (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4,
                out T _5, out T _6, out T _7, out T _8, out T _9) =>
                (_1, _2, _3, _4, _5, _6, _7, _8, _9) =
                (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), 
                    _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
            public void Deconstruct(out T _1, out T _2, out T _3, out T _4, 
                out T _5, out T _6, out T _7, out T _8, out T _9, out T _10) =>
                (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) = 
                (_sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(),
                    _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read(), _sc.Read());
        }

        private readonly Deconstructor _deconstructor;

        public T Read() => Next<T>();
        public Deconstructor ReadMulti() => _deconstructor;
        IEnumerable<T> Enumerable(int N) {
            for (int i = 0; i < N; ++i) yield return Read();
        }
        public T[] ReadArray(in int N) => Enumerable(N).ToArray();
        public List<T> ReadList(in int N) => Enumerable(N).ToList();
        public T[,] ReadArray2d(in int N, in int M) => next2DArray<T>(N, M);
        public T[][] ReadListArray(in int n) {
            var ret = new T[n][];
            for (int i = 0; i < n; i++) ret[i] = ReadArray(Next<int>());
            return ret;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    class LengthAttribute : Attribute
    {
        public LengthAttribute(string len) => LengthName = len;
        public string LengthName { get; }
    }
    public static class Input
    {
        const char _separator = ' ';
        static readonly Queue<string> _input = new Queue<string>();
        static readonly StreamReader sr =
#if FILE
            new StreamReader("in.txt");
#else
            new StreamReader(Console.OpenStandardInput());
#endif

        public static string ReadLine => sr.ReadLine();
        static string ReadStr() => Next();
        static int ReadInt() => int.Parse(Next());
        static long ReadLong() => long.Parse(Next());
        static ulong ReadULong() => ulong.Parse(Next());
        static double ReadDouble() => double.Parse(Next());
        static BigInteger ReadBigInteger() => BigInteger.Parse(Next());

        public static string Next() {
            if (_input.Any()) return _input.Dequeue();
            foreach (var val in sr.ReadLine().Split(_separator)) _input.Enqueue(val);
            return _input.Dequeue();
        }
        public static T Next<T>() =>
            default(T) switch {
                sbyte _ => (T) (object) (sbyte) ReadInt(),
                short _ => (T) (object) (short) ReadInt(),
                int _ => (T) (object) ReadInt(),
                long _ => (T) (object) ReadLong(),
                byte _ => (T) (object) (byte) ReadULong(),
                ushort _ => (T) (object) (ushort) ReadULong(),
                uint _ => (T) (object) (uint) ReadULong(),
                ulong _ => (T) (object) ReadULong(),
                float _ => (T) (object) (float) ReadDouble(),
                double _ => (T) (object) ReadDouble(),
                string _ => (T) (object) ReadStr(),
                char _ => (T) (object) ReadStr()[0],
                BigInteger _ => (T) (object) ReadBigInteger(),
                _ => typeof(T) == typeof(string)
                    ? (T) (object) ReadStr()
                    : throw new NotSupportedException(),
            };
        public static (T, U) Next<T, U>() => (Next<T>(), Next<U>());
        public static (T, U, V) Next<T, U, V>() => (Next<T>(), Next<U>(), Next<V>());
        public static (T, U, V, W) Next<T, U, V, W>() => (Next<T>(), Next<U>(), Next<V>(), Next<W>());
        public static (T, U, V, W, X) Next<T, U, V, W, X>() => (Next<T>(), Next<U>(), Next<V>(), Next<W>(), Next<X>());
        public static T[] nextArray<T>(in int size) {
            var ret = new T[size]; for (int i = 0; i < size; ++i) ret[i] = Next<T>(); return ret;
        }
        public static T[,] next2DArray<T>(int n, in int m) {
            var ret = new T[n, m];
            for (int i = 0; i < n; ++i) for (int j = 0; j < m; ++j) ret[i, j] = Next<T>(); return ret;
        }
        public static (T[], U[]) nextArray<T, U>(in int size) {
            var ret1 = new T[size]; var ret2 = new U[size];
            for (int i = 0; i < size; ++i) (ret1[i], ret2[i]) = Next<T, U>();
            return (ret1, ret2);
        }
        public static (T[], U[], V[]) nextArray<T, U, V>(in int size) {
            var ret1 = new T[size]; var ret2 = new U[size]; var ret3 = new V[size];
            for (int i = 0; i < size; ++i) (ret1[i], ret2[i], ret3[i]) = Next<T, U, V>();
            return (ret1, ret2, ret3);
        }
        public static (T[], U[], V[], W[]) nextArray<T, U, V, W>(in int size) {
            var ret1 = new T[size]; var ret2 = new U[size];
            var ret3 = new V[size]; var ret4 = new W[size];
            for (int i = 0; i < size; ++i) (ret1[i], ret2[i], ret3[i], ret4[i]) = Next<T, U, V, W>();
            return (ret1, ret2, ret3, ret4);
        }
    
    
        public static T Read<T>() {
            var type = typeof(T);
            if ((!type.IsValueType || type.IsPrimitive || type.IsEnum) && !type.IsClass) throw new Exception("T must be a class.");

            var constructor = type.GetConstructors()[0];
            var ret = default(T);

            object ReadInner(Type t) {
                if (!t.FullName!.StartsWith("System.ValueTuple")) {
                    var method = typeof(Input).GetMethods()
                        .SingleOrDefault(method =>
                            method.Name == nameof(Next) && method.GetGenericArguments().Length == 1
                        )!.MakeGenericMethod(t);
                    return method.Invoke(null, null);
                }
                var tupleTypes = t.GetGenericArguments();
                if (tupleTypes.Length > 8) {
                    throw new Exception("Tuple length must be less than 8.");
                }
                var tuple = new object[tupleTypes.Length];
                for (int i = 0; i < tupleTypes.Length; i++) {
                    tuple[i] = ReadInner(tupleTypes[i]);
                }
                return Activator.CreateInstance(t, tuple);
            }

            var param = new List<object>();
            var dict = new Dictionary<string, object>();
            foreach (var p in constructor.GetParameters()) {
                var pt = p.ParameterType;
                if (pt.IsArray) {
                    var len = p.GetCustomAttribute<LengthAttribute>();
                    if (len is null) {
                        throw new Exception($"LengthAttribute for {p.Name} is not specified.");
                    }
                    if (!dict.ContainsKey(len.LengthName) || dict[len.LengthName] is not int length) {
                        throw new Exception($"Type of {len.LengthName} must be int as length of {p.Name}.");
                    }
                    // if (len.Converter != null) length = len.Converter(length);
                    var pt2 = pt.GetElementType()!;
                    var array = Array.CreateInstance(pt2, length);
                    for (int i = 0; i < length; i++) {
                        array.SetValue(ReadInner(pt2), i);
                    }
                    param.Add(array);
                    continue;
                }
                if (pt.FullName!.StartsWith("System.Collections.Generic.List`1")) {
                    var pt2 = pt.GenericTypeArguments[0];
                    var length = (int) ReadInner(typeof(int));
                    var array = Array.CreateInstance(pt2, length);
                    for (int i = 0; i < length; i++) {
                        array.SetValue(ReadInner(pt2), i);
                    }
                    var toList = typeof(Enumerable)
                        .GetMethod("ToList", BindingFlags.Public | BindingFlags.Static)!
                        .MakeGenericMethod(pt2);
                
                    param.Add(Activator.CreateInstance(pt, array));
                    continue;
                }

                var value = ReadInner(pt);
                param.Add(value);
                dict[p.Name ?? "(null)"] = value;
            }
            return (T) constructor.Invoke(param.ToArray());
        }
    }
    public static class Output
    {
        [MethodImpl(256)] public static void Print() => Console.WriteLine();
        [MethodImpl(256)] public static void Print(in string s, bool endline = true) {
            if (endline) Console.WriteLine(s); else Console.Write(s);
        }
        [MethodImpl(256)] public static void Print(in char s, bool endline = true) {
            if (endline) Console.WriteLine(s); else Console.Write(s);
        }
        [MethodImpl(256)] public static void Print(in int v, bool endline = true) {
            if (endline) Console.WriteLine(v); else Console.Write(v);
        }
        [MethodImpl(256)] public static void Print(in long v, bool endline = true) {
            if (endline) Console.WriteLine(v); else Console.Write(v);
        }
        [MethodImpl(256)] public static void Print(in ulong v, bool endline = true) {
            if (endline) Console.WriteLine(v); else Console.Write(v);
        }
        [MethodImpl(256)] public static void Print(in bool b) => PrintBool(b);
        [MethodImpl(256)] public static void Print(in object v) => Console.WriteLine(v);
        [MethodImpl(256)] public static void Print<T>(in IEnumerable<T> array, string separator = " ") =>
            Console.WriteLine(array.Join(separator));
        [MethodImpl(256)] public static void MultiPrint<T>(params T[] t) => Print(t);

        [MethodImpl(256)] public static void PrintExit(in string s, bool endline = true, int exitCode = 0) {
            if (endline) Console.WriteLine(s); else Console.Write(s);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit(in char s, bool endline = true, int exitCode = 0) {
            if (endline) Console.WriteLine(s); else Console.Write(s);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit(in int v, bool endline = true, int exitCode = 0) {
            if (endline) Console.WriteLine(v); else Console.Write(v);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit(in long v, bool endline = true, int exitCode = 0) {
            if (endline) Console.WriteLine(v); else Console.Write(v);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit(in ulong v, bool endline = true, int exitCode = 0) {
            if (endline) Console.WriteLine(v); else Console.Write(v);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit(in bool b, int exitCode = 0) {
            PrintBool(b);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit(in object v, int exitCode = 0) {
            Console.WriteLine(v);
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void PrintExit<T>(in IEnumerable<T> array, string separator = " ", int exitCode = 0) {
            Console.WriteLine(array.Join(separator));
            Environment.Exit(exitCode);
        }

#if LOCAL
        [MethodImpl(256)] public static void DebugPrint<T>(in T value, bool endline = true) {
            if (endline) Console.WriteLine(value); else Console.Write(value);
        }
#else
        public static void debug(params object[] obj) { }
#endif

        [MethodImpl(256)] static void PrintBool(in bool val, in string yes = null, in string no = null) =>
            Print(val ? yes ?? _yes : no ?? _no);
        static string _yes = "Yes", _no = "No";

        public static void SetYesNoString(in YesNoType t) => (_yes, _no) = YesNoString[t];
        public static void SetYesNoString(in string yes, in string no) => (_yes, _no) = (yes, no);

        static readonly Dictionary<YesNoType, (string yes, string no)>
            YesNoString = new Dictionary<YesNoType, (string, string)> {
                {YesNoType.Default, ("Yes", "No")},
                {YesNoType.Yes_No, ("Yes", "No")},
                {YesNoType.YES_NO, ("YES", "NO")},
                {YesNoType.Upper, ("YES", "NO")},
                {YesNoType.yes_no, ("yes", "no")},
                {YesNoType.Lower, ("yes", "no")},
                {YesNoType.Possible_Impossible, ("Possible", "Impossible")},
                {YesNoType.Yay, ("Yay!", ":(")},
            };

        public static readonly (string yes, string no) YN_Possible = ("Possible", "Impossible"),
            YN_lower = ("yes", "no"), YN_upper = ("YES", "NO"), YN_Yay = ("Yay!", ":(");

        public static void Yes() => Print(_yes);
        public static void No() => Print(_no);
        [MethodImpl(256)] public static void YesExit(int exitCode = 0) {
            Yes();
            Console.Out.Flush();
            Environment.Exit(exitCode);
        }
        [MethodImpl(256)] public static void NoExit(int exitCode = 0) {
            No();
            Console.Out.Flush();
            Environment.Exit(exitCode);
        }

        public const string endl = "\n";

        public enum YesNoType { Default, Yes_No, YES_NO, Upper, yes_no, Lower, Possible_Impossible, Yay }
    }
    public class Program
    {
        public static void Main(string[] args) {
            var sw = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = false};
            Console.SetOut(sw);
            new Solver().Main(Read<In>());
            Console.Out.Flush();
        }
    }
    partial class Solver
    {
        private Scanner<int> Int { get; } = new();
        private Scanner<long> Long { get; } = new();
        private Scanner<string> String { get; } = new();
        const int INF = 1000000010;
        const long LINF = 1000000000000000100;
        const double EPS = 1e-8;
        public static readonly int[] dx = {-1, 0, 0, 1, -1, -1, 1, 1}, dy = {0, 1, -1, 0, -1, 1, -1, 1};
    }
}
