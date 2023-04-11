using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public static class ObjectExtentions
    {
        public static Action GetBaseConstructor<T>(this T obj) =>
    () => typeof(T)
          .BaseType
          .GetConstructor(Type.EmptyTypes)
          .Invoke(obj, Array.Empty<object>());


        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }
        public static IEnumerable Append(this IEnumerable first, params object[] second)
        {
            return first.OfType<object>().Concat(second);
        }
        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second)
        {
            return first.Concat(second);
        }
        public static IEnumerable Prepend(this IEnumerable first, params object[] second)
        {
            return second.Concat(first.OfType<object>());
        }

    }


}
