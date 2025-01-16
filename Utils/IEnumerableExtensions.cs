using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public static class IEnumerableExtensions
    {
      
        public static T SelectRandom<T>(this IEnumerable<T> list, Random rand)
        {
            return list.ElementAt(rand.Next(0, list.Count()));
        }
    }
