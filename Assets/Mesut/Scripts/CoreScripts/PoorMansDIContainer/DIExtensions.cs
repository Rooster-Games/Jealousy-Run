using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DI
{
    public static class DIExtensions
    {
        public static void CheckForIniting<T>(this T instance)
        {
            System.Type t = typeof(T);
            var initMethodInfo = t.GetMethod("Init");

            if (initMethodInfo != null)
                PMContainer.Instance.RegisterForIniting(instance);
        }
    }
}
