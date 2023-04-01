using System;
using System.Collections;
using System.Collections.Generic;
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
            set { _isExpanded = value; OnExpand(); OnPropertyChanged("IsExpanded"); }
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
            set { _parent = value; OnPropertyChanged("Parent"); }
        }
        private DataItemCollection _items;

        public DataItemCollection Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        private object  _attachedObject;
       // static private GetImageTextFrombldProjectObjectConvecter ObjecobjectTo_Urltext_Convectert = new GetImageTextFrombldProjectObjectConvecter();
        static private GetImageFrombldProjectObjectConvecter ObjecobjectTo_Urltext_Convectert = new GetImageFrombldProjectObjectConvecter();
        public object AttachedObject
        {
            get { return _attachedObject; }
            set {
                _attachedObject = value;
                if (_attachedObject != null)
                {
                    //Tuple<Uri,string> tuple = (Tuple<Uri, string>)ObjecobjectTo_Urltext_Convectert.Convert(_attachedObject, null, null, CultureInfo.CurrentCulture);
                    //ImageUrl = tuple.Item1;
                    //Text = tuple.Item2;

                }
                OnSetAttachedObject(); 
                OnPropertyChanged("AttachedObject"); 
                OnPropertyChanged("Type"); 
            }
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
