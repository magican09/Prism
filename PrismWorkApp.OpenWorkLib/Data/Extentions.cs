using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public static class ObjectExtentions
    {
        public static Action GetBaseConstructor<T>(this T obj) =>
    () => typeof(T)
          .BaseType
          .GetConstructor(Type.EmptyTypes)
          .Invoke(obj, Array.Empty<object>());

        public static int Remove<TEntity>(this Stack<TEntity> stack, TEntity item) //where TEntity:IComparable
        {
            Stack<TEntity> buffer_stack = new Stack<TEntity>();
            TEntity buffer;
            int removed_items_counts = 0;
            for (int ii = 0; ii < stack.Count; ii++)
            {
                buffer = stack.Pop();
                if (!buffer.Equals(item)) { buffer_stack.Push(item); removed_items_counts++; }
            }
            while (buffer_stack.Count > 0)
            {
                stack.Push(buffer_stack.Pop());
            }
            return removed_items_counts;
        }
    }


}
