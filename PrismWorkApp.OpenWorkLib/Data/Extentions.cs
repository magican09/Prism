using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public static class ObjectExtentions
    {
        public static Action GetBaseConstructor<T>(this T obj) =>
    () => typeof(T)
          .BaseType
          .GetConstructor(Type.EmptyTypes)
          .Invoke(obj, Array.Empty<object>());
    }
    
}
