using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEntityObject : INameable,  IJornalable
    {

        public Guid Id { get; set; }
        public Guid StoredId { get; set; }
        public  Func<IEntityObject, bool> RestrictionPredicate { get; set; }
        public bool CopingEnable { get; set; }
    }
}
