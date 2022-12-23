using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchical
    {
        public IBindableBase Parent { get; set; }
        public ObservableCollection<IBindableBase> Children { get; set; }
    }
}
