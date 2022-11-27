using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ICuntextIdable
    {
        Guid CurrentContextId { get; set; }
    }
}
