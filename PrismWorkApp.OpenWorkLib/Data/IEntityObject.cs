using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : INameable, IJornalable, IKeyable, IValidateable
    {
        public Func<IEntityObject, bool> RestrictionPredicate { get; set; }
        public Guid StoredId { get; set; }
    }
}
