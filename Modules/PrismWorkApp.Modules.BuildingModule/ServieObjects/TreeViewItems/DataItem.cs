using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule
{
    public delegate void DataItemInitDelegateHandler(DataItem dataItem, object sender, PropertyChangedEventArgs e);
    public delegate void AttachedCollectionChangedDelegateHandler(DataItem dataItem, object sender, NotifyCollectionChangedEventArgs e);
    public delegate void MenuItemExpandDelegateHandler(DataItem dataItem);

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
        public DataItemInitDelegateHandler  DataItemInit;
        public   AttachedCollectionChangedDelegateHandler AttachedObjectCollectionChanged;
       // public MenuItemExpandDelegateHandler MenuItemExpand;
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

        private string _propName;

        public string PropName
        {
            get { return _propName; }
            set { _propName = value; OnPropertyChanged("PropName"); }
        }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value;
                if (_isExpanded)
                    OnMenuItemExpand(this);
                else
                    OnMenuItemFolded(this);
                OnPropertyChanged("IsExpanded"); }
        }
        private Uri _imageUrl;
        public Uri ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged("ImageUrl"); }
        }
        private DataItem _parent;
        public DataItem Parent
        {
            get { return _parent; }
            set {
                _parent = value; 

                OnPropertyChanged("Parent"); }
        }
        private DataItemCollection _items;

        public DataItemCollection Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        private object  _attachedObject;
       public object AttachedObject
        {
            get { return _attachedObject; }
            set {
                _attachedObject = value;
                if (_attachedObject != null)
                {
                    if (_attachedObject is INotifyPropertyChanged notifyable_object)
                        notifyable_object.PropertyChanged+=OnAttachedObjectPropertyChanged;
                    if(_attachedObject is INotifyCollectionChanged notifyable_collection)
                        notifyable_collection.CollectionChanged += OnAttachedCollectionChanged;
                    if(_attachedObject is IList attached_collection)
                    {
                        foreach(object obj in attached_collection)
                        {

                        }
                    }
                    OnAttachedObjectPropertyChanged(this, new PropertyChangedEventArgs("AttachedObject"));
                }
                OnPropertyChanged("AttachedObject"); 
                OnPropertyChanged("Type"); 
            }
        }
        private void OnMenuItemFolded(DataItem dataItem)
        {

        }

       private void  OnMenuItemExpand(DataItem dataItem)
        {
           
            foreach (DataItem item in dataItem.Items)
            {
                item.OnAttachedObjectPropertyChanged(item,new PropertyChangedEventArgs("IsExpanded"));
            }
        }
        public  void OnAttachedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Add &&  IsExpanded)
            //{
            //    foreach (object obj in e.NewItems)
            //    {
            //        if (Items.Where(itm => itm.AttachedObject == obj).FirstOrDefault() == null)
            //        {
            //            DataItem new_item = new DataItem();
            //            new_item.AttachedObject = obj;
            //       //     Items.Add(new_item);
            //        }
            //    }
            //}
            if (Parent != null && Parent.IsExpanded) { Parent.IsExpanded = false; }
            AttachedObjectCollectionChanged?.Invoke(this, sender, e);
            OnAttachedObjectPropertyChanged(this, new PropertyChangedEventArgs("AttachedCollectionChanged"));
           
        }

        public void OnAttachedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.AttachedObject == null) return;
            this.Items.Clear();
            if(DataItemInit==null)
                throw new Exception($"Не установлен оработчик инициализации DataItemInit {this.ToString()} объекта {this.AttachedObject.ToString()}");  
            DataItemInit?.Invoke(this, sender, e);
            
        }

        private Type _type;

        public Type Type
        {
            get { if (AttachedObject != null)
                    _type = AttachedObject.GetType();
                return _type;
            }
            set { _type = value; OnPropertyChanged("Type"); OnPropertyChanged("TypeName"); }
        }
        private string _typeName;

        public string TypeName
        {
            get
            {
                if (Type != null)
                    _typeName = Type.Name;
                return _typeName;
            }
            set { _typeName = value; OnPropertyChanged("TypeName"); }
        }
        private void OnSetAttachedObject()
        {
            var prop_infoes = AttachedObject.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach(PropertyInfo prop_info in prop_infoes)
            {
                var prop_val = prop_info.GetValue(AttachedObject);
                if (!prop_info.PropertyType.FullName.Contains("System."))
                {
                   if ((prop_val is IEntityObject))
                    {
                        DataItem dataItem = new DataItem();
                        dataItem.PropName = prop_info.Name;
                        dataItem.Type = prop_info.PropertyType;
                        if (prop_val is IList list_prop)
                        {
                            foreach(object obj in  list_prop)
                            {
                                DataItem in_dataItem = new DataItem();
                                in_dataItem.PropName = prop_info.Name;
                                in_dataItem.Type = prop_info.PropertyType;
                            }
                        }
                        Items.Add(dataItem);



                    }
                }
            }    
        }

        private void OnExpand()
        {
            foreach (DataItem item in Items)
            {
               var obj = AttachedObject.GetType().GetProperty(item.PropName).GetValue(AttachedObject);
                if (obj != null) item.AttachedObject = obj;
            }
        }
    }
}
