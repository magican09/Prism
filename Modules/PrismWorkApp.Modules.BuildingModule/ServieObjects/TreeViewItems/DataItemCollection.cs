using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class DataItemCollection : ObservableCollection<DataItem>, INotifyPropertyChanged
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
        public DataItemCollection()
        {

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
            if (this.Owner != null)
            {
                //   item.AttachedObjectCollectionChanged += this.Owner.AttachedObjectCollectionChanged;
                item.DataItemInit += this.Owner.DataItemInit;
                //  item.MenuItemExpand += this.Owner.MenuItemExpand;

            }
            base.SetItem(index, item);
        }
        protected override void ClearItems()
        {
            foreach (DataItem item in this)
            {
                item.Parent = null;
                if (this.Owner != null)
                {
                    //   item.AttachedObjectCollectionChanged -= this.Owner.AttachedObjectCollectionChanged;
                    //  item.DataItemInit -= this.Owner.DataItemInit;
                    //    item.MenuItemExpand -= this.Owner.MenuItemExpand;
                }
            }
            base.ClearItems();
        }
        protected override void InsertItem(int index, DataItem item)
        {
            item.Parent = this.Owner;
            if (this.Owner != null)
            {
                // item.AttachedObjectCollectionChanged += this.Owner.AttachedObjectCollectionChanged;
                item.DataItemInit += this.Owner.DataItemInit;
                //    item.MenuItemExpand += this.Owner.MenuItemExpand;
            }

            if (!this.Contains(item))
                base.InsertItem(index, item);

        }
        protected override void RemoveItem(int index)
        {
            this[index].Parent = null;
            if (this.Owner != null)
            {
                // this[index].AttachedObjectCollectionChanged -= this.Owner.AttachedObjectCollectionChanged;
                this[index].DataItemInit -= this.Owner.DataItemInit;
                //  this[index].MenuItemExpand -= this.Owner.MenuItemExpand;
            }
            base.RemoveItem(index);
        }
    }
}
