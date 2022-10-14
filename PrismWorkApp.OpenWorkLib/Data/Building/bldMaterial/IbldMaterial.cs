using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldMaterial:IRegisterable,IMeasurable
    {
        public bldDocumentsGroup  Documents { get; set; }

    }
}
