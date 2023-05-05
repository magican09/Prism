using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
   public static  class DbContextExtentions
    {
        public static IQueryable Set(this DbContext context, Type T)
        {
            // Get the generic type definition
            // MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);
            MethodInfo method = typeof(DbContext).GetMethods().Where(m=>m.Name== nameof(DbContext.Set)).FirstOrDefault();
          
            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return method.Invoke(context, null) as IQueryable;
        }

        //public static IQueryable<T> Set<T>(this DbContext context)
        //{
        //    // Get the generic type definition 
        //    MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);

        //    // Build a method with the specific type argument you're interested in 
        //    method = method.MakeGenericMethod(typeof(T));

        //    return method.Invoke(context, null) as IQueryable<T>;
        //}
    }
}
