using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Utils
{
    public sealed class Numeric
    {
        private static readonly List<Type> PossibleTypes = new List<Type>
        {
            typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), 
            typeof(float), typeof(double), typeof(decimal), typeof(Vector2), typeof(UnityEngine.Vector3), typeof(Vector4), typeof(Quaternion), 
        };

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool Is(object obj)
        {
            if (obj == null) return false;

            var objType = obj;

            return PossibleTypes.Any(t => objType == t);
        }

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool Is<T>()
        {
            return Is(typeof(T));
        }
    }
}