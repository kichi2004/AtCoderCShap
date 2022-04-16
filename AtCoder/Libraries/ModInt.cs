using System;
using System.Diagnostics;

namespace Solve.Libraries.ModInt
{
    [DebuggerDisplay("{" + nameof(_value) + "}")]
    public struct ModInt : IEquatable<ModInt>, IComparable<ModInt> {
        // public const int MOD = 1_000_000_007;
        public const int MOD = 998_244_353;
        long _value;
        public static readonly ModInt Zero = new ModInt(0);
        public static readonly ModInt One = new ModInt(1);
        public ModInt(in long value) => _value = (value % MOD + MOD) % MOD;
        public int Value => (int) _value;
        public ModInt Invert => ModPow(this, MOD - 2);
        public static ModInt operator -(ModInt value) {
            value._value = MOD - value._value;
            return value;
        }
        public static ModInt operator +(ModInt left, in ModInt right) {
            left._value += right._value;
            if (left._value >= MOD) left._value -= MOD;
            return left;
        }
        public static ModInt operator -(ModInt left, in ModInt right) {
            left._value -= right._value;
            if (left._value < 0) left._value += MOD;
            return left;
        }
        public static ModInt operator *(ModInt left, in ModInt right) => left._value * right._value % MOD;
        public static ModInt operator /(ModInt left, in ModInt right) => left * right.Invert;
        public static ModInt operator ++(ModInt value) {
            if (value._value == MOD - 1) value._value = 0;
            else value._value++;
            return value;
        }
        public static ModInt operator --(ModInt value) {
            if (value._value == 0) value._value = MOD - 1;
            else value._value--;
            return value;
        }
        public static bool operator ==(ModInt left, in ModInt right) => left.Equals(right);
        public static bool operator !=(ModInt left, in ModInt right) => !left.Equals(right);
        public static implicit operator ModInt(in int value) => new ModInt(value);
        public static implicit operator ModInt(in long value) => new ModInt(value);
        public static explicit operator int(ModInt m) => m.Value;
        public static ModInt ModPow(ModInt value, long exponent) {
            var r = new ModInt(1);
            for (; exponent > 0; value *= value, exponent >>= 1)
                if ((exponent & 1) == 1)
                    r *= value;
            return r;
        }
        public static ModInt ModFact(int value) {
            var r = new ModInt(1);
            for (var i = 2; i <= value; i++) r *= value;
            return r;
        }
        public bool Equals(ModInt other) => _value == other._value;
        public override bool Equals(object obj) => obj is ModInt m && Equals(m);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public int CompareTo(ModInt other) => _value.CompareTo(other._value);
    }
}