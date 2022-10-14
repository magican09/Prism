using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstructionCompanyGroup : INotifyCollectionChanged, IEnumerable<bldConstructionCompany>, 
                                IList<bldConstructionCompany>,INameable, ICollection<bldConstructionCompany>,IEntityObject
    {

    }
}
