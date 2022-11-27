using PrismWorkApp.OpenWorkLib.Data;
using System;

namespace PrismWorkApp.OpenWorkLib.Core
{
    public static class ExtentionMethods
    {
        public static TSourse GetCopy<TSourse>(this TSourse sourse, Func<TSourse, bool> prediacate)
              where TSourse : IEntityObject, new()
        {
            TSourse target = new TSourse();
            Functions.CopyObjectReflectionNewInstances<TSourse>(sourse, target, prediacate);

            return target;
        }
        public static IEntityObject GetCopy(this IEntityObject sourse, Func<IEntityObject, bool> prediacate)
        {
            IEntityObject target = (IEntityObject)Activator.CreateInstance(sourse.GetType());
            Functions.CopyObjectReflectionNewInstances<IEntityObject>(sourse, target, prediacate);

            return target;
        }
    }
}
