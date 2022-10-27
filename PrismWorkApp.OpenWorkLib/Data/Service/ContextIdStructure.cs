using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   public  class ContextIdStructure
    {
        public Guid ContextId { get; set; }
        public ContextIdStructure Children { get; set; }
        public ContextIdStructure Parent { get; set; }

    }
}
