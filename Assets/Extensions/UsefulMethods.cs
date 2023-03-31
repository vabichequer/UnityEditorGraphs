using System.Linq;
using UnityEngine;

namespace Extensions
{
    public class UsefulMethods
    {
        public static void Print(params object[] args)
        {
            var message = args.Aggregate(string.Empty, (current, t) => current + t);
            Debug.Log(message);
        }
    }
}