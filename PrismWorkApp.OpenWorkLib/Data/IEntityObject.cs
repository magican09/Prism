using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : INameable, IJornalable
    {

        public Guid Id { get; set; }
        public Guid StoredId { get; set; }
        public Func<IEntityObject, bool> RestrictionPredicate { get; set; }
        public bool CopingEnable { get; set; }
    }
}
