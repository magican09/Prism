using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace PrismWorkApp.Modules.BuildingModule
{
    public delegate void DataItemInitDelegateHandler(DataItem dataItem, object sender, PropertyChangedEventArgs e);
    public delegate void AttachedCollectionChangedDelegateHandler(DataItem dataItem, object sender, NotifyCollectionChangedEventArgs e);
    public delegate void MenuItemExpandDelegateHandler(DataItem dataItem);

    [ContentProperty("Children")]
    public class DataItem : DependencyObject, INotifyPropertyChanged,IKeyable
    {
    
    private Guid _id;
    [CreateNewWhenCopy]
    public Guid Id
    {
        get { return _id; }
        set {  _id= value; OnPropertyChanged("Id"); }
    }
        private static Dictionary<object, DataItem> _allItems = new Dictionary<object, DataItem>();

        public  Dictionary<object, DataItem> AllItems
        {
            get { return _allItems; }
            set { _allItems = value; OnPropertyChanged("AllItems"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public DataItemInitDelegateHandler DataItemInit;
        public AttachedCollectionChangedDelegateHandler AttachedObjectCollectionChanged;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register(
                    "Text",
                    typeof(string),
                    typeof(DataItem),
                    new FrameworkPropertyMetadata(
                        string.Empty));

        ///////////////////////////////////////////////////////////
        ///
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataItem), new PropertyMetadata(new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as DataItem;
            if (control != null)
                control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }



        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }

        }

        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Do your stuff here.
        }



        /// <summary>
        /// /////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _isHaveChanges;

        public bool IsHaveChanges
        {
            get { return _isHaveChanges; }
            set { _isHaveChanges = value; OnPropertyChanged("IsHaveChanges"); }
        }

        private DataItemCollection _items;

        public DataItemCollection Items
        {
            get { return _items; }
            set { _items = value; }
        }
                    

        // public MenuItemExpandDelegateHandler MenuItemExpand;
        public DataItem()
        {
             this._items = new DataItemCollection(this);
            


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
            set
            {
                _isExpanded = value;
                if (_isExpanded)
                    OnMenuItemExpand(this);
                else
                    OnMenuItemFolded(this);
                OnPropertyChanged("IsExpanded");
            }
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
            set
            {
                _parent = value;

                OnPropertyChanged("Parent");
            }
        }

        

        private object _attachedObject;
        public object AttachedObject
        {
            get { return _attachedObject; }
            set
            {
                _attachedObject = value;
                if (_attachedObject != null)
                {
                    if (_attachedObject is INotifyPropertyChanged notifyable_object)
                        notifyable_object.PropertyChanged += OnAttachedObjectPropertyChanged;
                    if (_attachedObject is INotifyCollectionChanged notifyable_collection)
                        notifyable_collection.CollectionChanged += OnAttachedCollectionChanged;
                    if (_attachedObject is IList attached_collection)
                    {
                        foreach (object obj in attached_collection)
                        {

                        }
                    }
                    _allItems.Add(_attachedObject, this);
                    OnAttachedObjectPropertyChanged(this, new PropertyChangedEventArgs("AttachedObject"));
                }
                OnPropertyChanged("AttachedObject");
                OnPropertyChanged("Type");
            }
        }
        private void OnMenuItemFolded(DataItem dataItem)
        {

        }

        private void OnMenuItemExpand(DataItem dataItem)
        {

            foreach (DataItem item in dataItem.Items)
            {
                item.OnAttachedObjectPropertyChanged(item, new PropertyChangedEventArgs("IsExpanded"));
            }
        }
        public void OnAttachedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            //  this.Items.Clear();
            if (DataItemInit == null)
                throw new Exception($"Не установлен оработчик инициализации DataItemInit {this.ToString()} объекта {this.AttachedObject.ToString()}");
            DataItemInit?.Invoke(this, sender, e);

        }

        private Type _type;

        public Type Type
        {
            get
            {
                if (AttachedObject != null)
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
            foreach (PropertyInfo prop_info in prop_infoes)
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
                            foreach (object obj in list_prop)
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
