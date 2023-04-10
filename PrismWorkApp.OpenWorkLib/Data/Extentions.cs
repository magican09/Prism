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

        public static int Remove<TEntity>(this Stack<TEntity> stack, TEntity item) where TEntity:IKeyable
        {
                Stack<TEntity> buffer_stack = new Stack<TEntity>();
            TEntity buffer_obj;
            int removed_items_counts = 0;
            int stack_count = stack.Count;
            for (int ii = 0; ii < stack_count; ii++)
            {
                buffer_obj = stack.Pop();
                if (buffer_obj.Id!=item.Id) 
                { 
                    buffer_stack.Push(buffer_obj); 
                  
                }
                else removed_items_counts++;
            }

            while (buffer_stack.Count > 0)
            {
                stack.Push(buffer_stack.Pop());
            }
            if (removed_items_counts==0)
                 throw new Exception("Eroor when Remove from Stack - elemtn not found in Stack!!");

            return removed_items_counts;
        }
    }


}
