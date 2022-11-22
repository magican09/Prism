using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IKeyable
    {
        public Guid Id { get; set; }
        public Guid StoredId {get;set; }
    }
}
