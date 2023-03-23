using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   public class EntityCategory : BindableBase
    {
        public Guid? ParentId { get; set; }

        private ObservableCollection<EntityCategory> _entities = new ObservableCollection<EntityCategory>();
        public ObservableCollection<EntityCategory> Children
        {
            get { return _entities; }
            set { SetProperty(ref _entities, value); }
        }
    }
}
