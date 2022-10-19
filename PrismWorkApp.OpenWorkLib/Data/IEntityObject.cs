using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : INameable, ILevelable, IJornalable, ICopingEnableable
    {

        public Guid Id { get; set; }
        public Guid StoredId { get; set; }
        public IJornalable ParentObject { get; set; }
        public  Func<IEntityObject, bool> RestrictionPredicate { get; set; }
    }
}
