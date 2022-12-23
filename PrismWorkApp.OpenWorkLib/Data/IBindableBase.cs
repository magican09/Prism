using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IBindableBase:INameable,IJornalable,IKeyable,ICodeable,IValidateable, IHierarchical,INotifyPropertyChanged
    {
        public Func<IEntityObject, bool> RestrictionPredicate { get; set; }
    }
}
