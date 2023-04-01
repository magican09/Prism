using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule 
{
    public class DataItemCollection:ObservableCollection<DataItem>,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public DataItemCollection(DataItem owner)
        {
            Owner = owner;
        }
        private DataItem _owner;

        public DataItem Owner
        {
            get { return _owner; }
            set { _owner = value; OnPropertyChanged("Owner"); }
        }
        protected override void SetItem(int index, DataItem item)
        {
            item.Parent = this.Owner;
            base.SetItem(index, item);
        }
        protected override void ClearItems()
        {
            foreach (DataItem item in this)
                item.Parent = null;
            base.ClearItems();
        }
        protected override void InsertItem(int index, DataItem item)
        {
            item.Parent = this.Owner;
            base.InsertItem(index, item);

        }
        protected override void RemoveItem(int index)
        {
            this[index].Parent = null;
            base.RemoveItem(index);
        }
    }
}
