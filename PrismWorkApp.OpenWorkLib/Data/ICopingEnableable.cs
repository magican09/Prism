using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ICopingEnableable
    {
          bool CopingEnable { get; set; }
          object Clone<TSourse>(Func<TSourse, bool> predicate) where TSourse : IEntityObject;
    }
}
