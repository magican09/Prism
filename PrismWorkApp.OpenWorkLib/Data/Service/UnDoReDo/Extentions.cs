using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public static class UnDoReDoExtentions
    {
        public static int Remove<TEntity>(this Stack<TEntity> stack, TEntity item) where TEntity : IKeyable
        {
            Stack<TEntity> buffer_stack = new Stack<TEntity>();
            TEntity buffer_obj;
            int removed_items_counts = 0;
            int stack_count = stack.Count;
            for (int ii = 0; ii < stack_count; ii++)
            {
                buffer_obj = stack.Pop();
                if (buffer_obj.Id != item.Id)
                {
                    buffer_stack.Push(buffer_obj);

                }
                else removed_items_counts++;
            }

            while (buffer_stack.Count > 0)
            {
                stack.Push(buffer_stack.Pop());
            }
            if (removed_items_counts == 0)
                throw new Exception("Eroor when Remove from Stack - elemtn not found in Stack!!");

            return removed_items_counts;
        }
        public static void Insert<TEntity>(this Stack<TEntity> stack, int index, TEntity item) where TEntity : IKeyable
        {
            Stack<TEntity> buffer_stack = new Stack<TEntity>();
            int stack_count = stack.Count;
            for (int ii = 0; ii < stack.Count; ii++)
            {
                if (index == ii)
                {
                    buffer_stack.Push(item);

                }
                buffer_stack.Push(stack.Pop());
            }
            while (buffer_stack.Count > 0)
            {
                stack.Push(buffer_stack.Pop());
            }
        }

        public static IEnumerable<IJornalable> GetAllChangedObjects(this IJornalable obj)
        {
            IEnumerable<IJornalable> finded_objects = new List<IJornalable>();
            if (finded_objects.Contains(obj)) return null;
            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                {
                    finded_objects = finded_objects.Union(jornable_prop.GetAllChangedObjects());
                    if (jornable_prop is IList list_jornable_prop) //Если свойтво еще является и списком
                        foreach (object element in list_jornable_prop)
                            if (element is IJornalable jornable_element)
                                finded_objects = finded_objects.Union(jornable_element.GetAllChangedObjects());
                }
            }
            if (obj is IList list_obj) //Если регистрируемый элемент сам  является коллекцией
                foreach (IJornalable element in list_obj)
                    finded_objects = finded_objects.Union(element.GetAllChangedObjects());

            if (obj.ChangesJornal.Count > 0 && !finded_objects.Where(ob => ob.StoredId == obj.StoredId).Any())
                return finded_objects.Union(new List<IJornalable>() { obj });
            return finded_objects;
        }



        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> first, params T[] second)
        {
            return second.Concat(first);
        }
    }
}
