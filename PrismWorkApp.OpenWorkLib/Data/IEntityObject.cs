using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : IKeyable, INameable, IJornalable,  IValidateable
    {
        public Func<IEntityObject, bool> RestrictionPredicate { get; set; }
       
    }
}
