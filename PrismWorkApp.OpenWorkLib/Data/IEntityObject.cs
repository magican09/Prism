using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : INameable, IJornalable, IKeyable, IValidateable,IHierarchical
    {
        public Func<IEntityObject, bool> RestrictionPredicate { get; set; }
    }
}
