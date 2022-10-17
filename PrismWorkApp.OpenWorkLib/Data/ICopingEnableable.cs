using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ICopingEnableable
    {
        public bool CopingEnable { get; set; }
        public  object Clone<TSourse>(Func<TSourse, bool> predicate) where TSourse : IEntityObject;
    }
}
