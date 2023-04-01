using PrismWorkApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Markup;

namespace PrismWorkApp.Modules.BuildingModule
{
    [ContentProperty("Children")]
  public   class MenuItem : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged; 
		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
        public MenuItem()
        {
            this._items = new MenuItemCollection(this);
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged("ImageUrl"); }
        }
        private MenuItem _parent;
        public MenuItem Parent
        {
            get { return _parent; }
            set { _parent = value; OnPropertyChanged("Parent"); }
        }
        private MenuItemCollection _items;

        public MenuItemCollection Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        private object  _attachedObject;

        public object AttachedObject
        {
            get { return _attachedObject; }
            set { _attachedObject = value; OnPropertyChanged("AttachedObject"); }
        }
        private INotifyCommand _command;

        public INotifyCommand Command
        {
            get { return _command; }
            set { _command = value; }
        }

     
        }

    
}
