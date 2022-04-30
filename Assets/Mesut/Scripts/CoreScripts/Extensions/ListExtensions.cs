using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCores
{
    public static class ListExtensions
    {
        public static T GetRandomItem<T>(this List<T> collection )
        {
            var randomIndex = Random.Range(0, collection.Count);
            return collection[randomIndex];
        }

        public static T GetRandomItem<T>(this T[] collection)
        {
            var randomIndex = Random.Range(0, collection.Length);
            return collection[randomIndex];
        }
    }
}
