using System;
using UnityEngine;

namespace Registry
{
    /// <summary>
    /// A <c>Guid</c> that can be serialized by Unity. The 128-bit <c>Guid</c> is stored as two 64-bit <c>ulong</c>s.
    /// More efficient than using a string.
    /// </summary>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>
    {
        /// <summary>
        /// Constructs a <see cref="SerializableGuid"/> from two 64-bit <c>ulong</c>s.
        /// </summary>
        /// <param name="guidLow">The low 8 bytes of the <c>Guid</c>.</param>
        /// <param name="guidHigh">The high 8 bytes of the <c>Guid</c>.</param>
        public SerializableGuid(ulong guidLow, ulong guidHigh)
        {
            this._guidLow = guidLow;
            this._guidHigh = guidHigh;
        }

        private static readonly SerializableGuid empty = new(0, 0);

        /// <summary>
        /// Used to represent <c>System.Guid.Empty</c> (that is, a GUID whose values are all zeros).
        /// </summary>
        public static SerializableGuid Empty => empty;

        /// <summary>
        /// Reconstructs the <c>Guid</c> from the serialized data.
        /// </summary>
        public readonly Guid Guid => Compose(_guidLow, _guidHigh);

        /// <summary>
        /// Generates a hash suitable for use with containers like `HashSet` and `Dictionary`.
        /// </summary>
        /// <returns>A hash code generated from this object's fields.</returns>
        public override int GetHashCode() => Combine(_guidLow.GetHashCode(), _guidHigh.GetHashCode());

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">The `object` to compare against.</param>
        /// <returns>`True` if <paramref name="obj"/> is of type <see cref="SerializableGuid"/> and
        /// <see cref="Equals(SerializableGuid)"/> also returns `true`; otherwise `false`.</returns>
        public override bool Equals(object obj) => (obj is SerializableGuid) && Equals((SerializableGuid)obj);

        /// <summary>
        /// Generates a string representation of the <c>Guid</c>. Same as <see cref="Guid"/><c>.ToString()</c>.
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.7.2#System_Guid_ToString">Microsoft's documentation</a>
        /// for more details.
        /// </summary>
        /// <returns>A string representation of the <c>Guid</c>.</returns>
        public override string ToString() => Guid.ToString();

        /// <summary>
        /// Generates a string representation of the <c>Guid</c>. Same as <see cref="Guid"/><c>.ToString(format)</c>.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of the <c>Guid</c>.
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.7.2#System_Guid_ToString_System_String_">Microsoft's documentation</a>
        /// for more details.</param>
        /// <returns>A string representation of the <c>Guid</c>.</returns>
        public string ToString(string format) => Guid.ToString(format);

        /// <summary>
        /// Generates a string representation of the <c>Guid</c>. Same as <see cref="Guid"/><c>.ToString(format, provider)</c>.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of the <c>Guid</c>.
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.7.2#System_Guid_ToString_System_String_System_IFormatProvider_">Microsoft's documentation</a>
        /// for more details.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of the <c>Guid</c>.</returns>
        public string ToString(string format, IFormatProvider provider) => Guid.ToString(format, provider);

        public int CompareTo(SerializableGuid other) => Guid.CompareTo(other.Guid);

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="other">The other <see cref="SerializableGuid"/> to compare against.</param>
        /// <returns>`True` if every field in <paramref name="other"/> is equal to this <see cref="SerializableGuid"/>, otherwise false.</returns>
        public bool Equals(SerializableGuid other)
        {
            return
                (_guidLow == other._guidLow) &&
                (_guidHigh == other._guidHigh);
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(SerializableGuid)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns>`True` if <paramref name="lhs"/> is equal to <paramref name="rhs"/>, otherwise `false`.</returns>
        public static bool operator ==(SerializableGuid lhs, SerializableGuid rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as `!`<see cref="Equals(SerializableGuid)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns>`True` if <paramref name="lhs"/> is not equal to <paramref name="rhs"/>, otherwise `false`.</returns>
        public static bool operator !=(SerializableGuid lhs, SerializableGuid rhs) => !lhs.Equals(rhs);

        public static implicit operator SerializableGuid(Guid guid)
        {
            (ulong low, ulong high) = Decompose(guid);
            return new SerializableGuid(low, high);
        }
        public static implicit operator Guid(SerializableGuid serializableGuid) => serializableGuid.Guid;

        [SerializeField]
        private ulong _guidLow;

        [SerializeField]
        private ulong _guidHigh;

        /// <summary>
        /// Reconstructs a [Guid](xref:System.Guid) from two <c>ulong</c>s representing the low and high bytes.
        /// </summary>
        /// <param name="low">The low 8 bytes of the guid</param>
        /// <param name="high">The high 8 bytes of the guid.</param>
        /// <returns>The Guid composed of <paramref name="low"/> and <paramref name="high"/>.</returns>
        public static Guid Compose(ulong low, ulong high)
        {
            return new Guid(
                (uint)((low & 0x00000000ffffffff) >> 0),
                (ushort)((low & 0x0000ffff00000000) >> 32),
                (ushort)((low & 0xffff000000000000) >> 48),
                (byte)((high & 0x00000000000000ff) >> 0),
                (byte)((high & 0x000000000000ff00) >> 8),
                (byte)((high & 0x0000000000ff0000) >> 16),
                (byte)((high & 0x00000000ff000000) >> 24),
                (byte)((high & 0x000000ff00000000) >> 32),
                (byte)((high & 0x0000ff0000000000) >> 40),
                (byte)((high & 0x00ff000000000000) >> 48),
                (byte)((high & 0xff00000000000000) >> 56));
        }

        struct GuidParts
        {
            public ulong low;
            public ulong high;
        }

        public static (ulong low, ulong high) Decompose(Guid guid)
        {
            unsafe
            {
                var parts = *(GuidParts*)&guid;
                return (parts.low, parts.high);
            }
        }
        public static int Combine(int hash1, int hash2)
        {
            unchecked
            {
                return hash1 * 486187739 + hash2;
            }
        }

        public static int ReferenceHash(object obj) => obj != null ? obj.GetHashCode() : 0;

        public static int Combine(int hash1, int hash2, int hash3) => Combine(Combine(hash1, hash2), hash3);
        public static int Combine(int hash1, int hash2, int hash3, int hash4) => Combine(Combine(hash1, hash2, hash3), hash4);
        public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5) => Combine(Combine(hash1, hash2, hash3, hash4), hash5);
        public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6) => Combine(Combine(hash1, hash2, hash3, hash4, hash5), hash6);
        public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6, int hash7) => Combine(Combine(hash1, hash2, hash3, hash4, hash5, hash6), hash7);
        public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6, int hash7, int hash8) => Combine(Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7), hash8);
        public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6, int hash7, int hash8, int hash9) =>
            Combine(Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7, hash8), hash9);
        public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6, int hash7, int hash8, int hash9, int hash10) =>
            Combine(Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7, hash8, hash9), hash10);
    }

    /// <summary>
    /// A generic serializable GUID.
    /// Would be nice to be able to use record struct when possible.
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    [Serializable]
    public struct SerializableGuid<T> : IComparable<SerializableGuid<T>>, IEquatable<SerializableGuid<T>>
    {
        static readonly SerializableGuid<T> k_Empty = new(Guid.Empty);
        public static SerializableGuid<T> Empty => k_Empty;

        public SerializableGuid Value { get; private set; }
        public SerializableGuid(Guid newGuid) => Value = newGuid;

        public int CompareTo(SerializableGuid<T> other) => Value.CompareTo(other.Value);
        public bool Equals(SerializableGuid<T> other) => Value.Equals(other.Value);
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is SerializableGuid<T> other && Equals(other);
        }
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
        public static bool operator ==(SerializableGuid<T> a, SerializableGuid<T> b) => a.CompareTo(b) == 0;
        public static bool operator !=(SerializableGuid<T> a, SerializableGuid<T> b) => !(a == b);
        public static bool operator <(SerializableGuid<T> left, SerializableGuid<T> right) => left.CompareTo(right) < 0;
        public static bool operator <=(SerializableGuid<T> left, SerializableGuid<T> right) => left.CompareTo(right) <= 0;
        public static bool operator >(SerializableGuid<T> left, SerializableGuid<T> right) => left.CompareTo(right) > 0;
        public static bool operator >=(SerializableGuid<T> left, SerializableGuid<T> right) => left.CompareTo(right) >= 0;
        public static implicit operator SerializableGuid<T>(Guid guid) => new(guid);
        public static implicit operator Guid(SerializableGuid<T> serializableGuid) => serializableGuid.Value.Guid;
    }
}
