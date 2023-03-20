using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResourseCategory:BindableBase
    {
        public ObservableCollection<bldResource> Resources { get; set; } = new ObservableCollection<bldResource>();
    }
}
