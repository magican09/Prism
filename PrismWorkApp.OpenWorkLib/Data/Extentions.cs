using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static bool ContainsDownWard(this IEntityObject entityObject, IEntityObject element)
        {
            if (entityObject.StoredId == element.Id) return true;
            if (entityObject is INameableObservableCollection coll_parent)
            {
                foreach (IEntityObject entity in coll_parent)
                {
                    if (entity.StoredId == element.StoredId || entity.ContainsDownWard(element))
                        return true;
                }
            }
            else
            {
                var prop_infoes = entityObject.GetType().GetProperties().Where(pr => pr.GetValue(entityObject) is IEntityObject);
                foreach (PropertyInfo prop_info in prop_infoes)
                {
                    IEntityObject prop_val = (IEntityObject)prop_info.GetValue(entityObject);
                    if (prop_val.StoredId == element.StoredId || prop_val.ContainsDownWard(element)) return true;

                }
            }
            return false;
        }
        public static bool ContainsUpWard(this IEntityObject entityObject, IEntityObject element)
        {
            if (entityObject.Parents == null) return false;
            if (entityObject.Parents.Where(pr => pr.StoredId == element.Id).Any()) return true;
            foreach (IEntityObject entity in entityObject.Parents)
            {
                if (entity.ContainsUpWard(element)) return true;
            }

            return false;
        }

    }


}
