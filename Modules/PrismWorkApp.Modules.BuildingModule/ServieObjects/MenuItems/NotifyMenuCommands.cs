using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class NotifyMenuCommands : ObservableCollection<INotifyCommand>, INotifyPropertyChanged,INameable
    {
        public string Name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private MenuItem _menuItem;

        public MenuItem MenuItem
        {
            get { return _menuItem; }
            set { _menuItem = value; OnPropertyChanged("MenuItem"); }
        }
        public NotifyMenuCommands()
        {
            MenuItem = new MenuItem();

        }
        protected override void SetItem(int index, INotifyCommand item)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Text = item.Name;
            menuItem.ImageUrl = item.ImageUri;
            menuItem.Command = item;
            MenuItem.Items.Add(menuItem);
            base.SetItem(index, item);
        }
        protected override void InsertItem(int index, INotifyCommand item)
        {
            if (!Items.Contains(item))
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Text = item.Name;
                menuItem.ImageUrl = item.ImageUri;
                menuItem.Command = item;
                MenuItem.Items.Add(menuItem);

                base.InsertItem(index, item);
            }
        }
    }
}
