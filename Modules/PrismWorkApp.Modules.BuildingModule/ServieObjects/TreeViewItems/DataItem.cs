using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Markup;

namespace PrismWorkApp.Modules.BuildingModule
{
    [ContentProperty("Children")]
  public   class DataItem:INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged; 
		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
        public DataItem()
        {
            this._items = new DataItemCollection(this);
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; OnPropertyChanged("IsExpanded"); }
        }
        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged("ImageUrl"); }
        }
        private DataItem _parent;
        public DataItem Parent
        {
            get { return _parent; }
            set { _parent = value; OnPropertyChanged("Parent"); }
        }
        private DataItemCollection _items;

        public DataItemCollection Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

    }
}
