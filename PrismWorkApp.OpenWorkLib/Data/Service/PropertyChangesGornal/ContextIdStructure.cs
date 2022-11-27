using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class ContextIdStructure
    {
        public Guid ContextId { get; set; }
        public ContextIdStructure Children { get; set; }
        public ContextIdStructure Parent { get; set; }

    }
}
