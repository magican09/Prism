using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IRegisterable : INameable, IKeyable
    {
        //public Guid Id { get; set; }
        //public Guid StoredId { get; set; }
        public string Code { get; set; }
        //public string ShortName { get; set; }
        //public string FullName { get; set; }

    }
}
