using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public  interface IbldMaterialsGroup : INotifyCollectionChanged, IEnumerable<bldMaterial>,
                            IList<bldMaterial>,INameable, ICollection<bldMaterial>
    {
        public decimal Cost { get; set; }
    }
}
