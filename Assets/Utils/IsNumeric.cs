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
            typeof(float), typeof(double), typeof(decimal), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector3), 
            typeof(UnityEngine.Vector4), typeof(UnityEngine.Quaternion)
        };

        private static readonly List<Type> VectorTypes = new List<Type>
        {
            typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector4), typeof(UnityEngine.Quaternion)
        };

        private static readonly List<int> VectorSizes = new List<int>
        {
            2, 3, 4, 4
        };

        /// <summary>
        /// Determines if a type is numeric and vectorized. Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static (bool, bool) Is(object obj)
        {
            if (obj == null) return (false, false);

            return (PossibleTypes.Any(t => obj == t), VectorTypes.Any(t => obj == t));
        }

        public static int Length(object obj)
        {
            if (obj == null) return -1;

            if (PossibleTypes.Any(t => obj == t)) return 1;

            foreach (var it in VectorTypes.Select((type, idx) => new { type, idx }))
            {
                if (it.type == obj)
                {
                    return VectorSizes[it.idx];
                }
            }

            return -1;
        }

        public static List<float> GetVector(object type, object obj)
        {
            if (type == typeof(UnityEngine.Vector2))
            {
                UnityEngine.Vector2 temp = (UnityEngine.Vector2)obj;
                return new List<float>() { temp.x, temp.y };
            }
            
            if (type == typeof(UnityEngine.Vector3))
            {
                UnityEngine.Vector3 temp = (UnityEngine.Vector3)obj;
                return new List<float>() { temp.x, temp.y, temp.z };
            }
            
            if (type == typeof(UnityEngine.Vector4))
            {
                UnityEngine.Vector4 temp = (UnityEngine.Vector4)obj;
                return new List<float>() { temp.x, temp.y, temp.z, temp.w };
            }
            
            if (type == typeof(UnityEngine.Quaternion))
            {
                UnityEngine.Quaternion temp = (UnityEngine.Quaternion)obj;
                return new List<float>() { temp.x, temp.y, temp.z, temp.w };
            }

            return null;
        }

            /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static (bool, bool) Is<T>()
        {
            return Is(typeof(T));
        }
    }
}