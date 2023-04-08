using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class MenuItemCollection : ObservableCollection<MenuItem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public MenuItemCollection(MenuItem owner)
        {
            Owner = owner;
        }
        private MenuItem _owner;

        public MenuItem Owner
        {
            get { return _owner; }
            set { _owner = value; OnPropertyChanged("Owner"); }
        }
        protected override void SetItem(int index, MenuItem item)
        {
            item.Parent = this.Owner;
            base.SetItem(index, item);
        }
        protected override void ClearItems()
        {
            foreach (MenuItem item in this)
                item.Parent = null;
            base.ClearItems();
        }
        protected override void InsertItem(int index, MenuItem item)
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
