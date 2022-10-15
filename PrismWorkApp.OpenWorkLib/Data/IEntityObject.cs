using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : INameable, ILevelable, IJornalable, ICopingEnableable
    {

        public Guid Id { get; set; }
        public Guid StoredId { get; set; }
        public bool CopingEnable{ get; set; }
        public object ParentObject { get; set; }
    }
}
