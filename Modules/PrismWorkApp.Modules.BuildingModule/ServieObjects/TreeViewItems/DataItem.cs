using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public delegate void DataItemInitDelegateHandler(DataItem dataItem, object sender, PropertyChangedEventArgs e);
    public delegate void AttachedCollectionChangedDelegateHandler(DataItem dataItem, object sender, NotifyCollectionChangedEventArgs e);
    public delegate void MenuItemExpandDelegateHandler(DataItem dataItem);

    [ContentProperty("Children")]
    public class DataItem : DependencyObject, INotifyPropertyChanged, IKeyable
    {

        private Guid _id;
        [CreateNewWhenCopy]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }
        private static Dictionary<object, DataItem> _allItems = new Dictionary<object, DataItem>();

        public Dictionary<object, DataItem> AllItems
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
        public DataItemCollection Items
        {
            get { return (DataItemCollection)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("ItemsSource", typeof(DataItemCollection), typeof(DataItem), new PropertyMetadata(new PropertyChangedCallback(OnItemsPropertyChanged)));

        private static void OnItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as DataItem;
            if (control != null)
                control.OnItemsChanged((DataItemCollection)e.OldValue, (DataItemCollection)e.NewValue);
        }

        public void SetItems(IEnumerable coll)
            {
            this.Items = new DataItemCollection(this);
            foreach (IEntityObject obj in coll)
            {
                DataItem new_dataItem = new DataItem();
                new_dataItem.AttachedObject = obj;
                DataItemsGenerator.Convert(new_dataItem, null, null, CultureInfo.CurrentCulture);
                this.Items.Add(new_dataItem);
            }
            if (coll is INotifyCollectionChanged notifyable_coll)
            {
                notifyable_coll.CollectionChanged +=OnAttachedObjectCollectionProperty_CollectionChanged;
            }
        }


        private void OnItemsChanged(DataItemCollection oldValue, DataItemCollection newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newItemsINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newItemsINotifyCollectionChanged_CollectionChanged);
            }

        }

        void newItemsINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
          
        }
        /// <summary>
        /// /////////////////////////////////////////////////////
        /// </summary>

        public IEntityObject AttachedObject
        {
            get { return (IEntityObject)GetValue(AttachedObjectProperty); }
            set { SetValue(AttachedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AttachedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachedObjectProperty =
            DependencyProperty.Register("AttachedObject", typeof(IEntityObject), typeof(DataItem), new PropertyMetadata(null,new PropertyChangedCallback(OnAttachedObjectProperty_Changed)));

        private static void OnAttachedObjectProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as DataItem;
            if (control != null)
                control.OnAttachedObjectChanged((IEntityObject)e.OldValue, (IEntityObject)e.NewValue);
        }
        private static DataItemsGenerationConverter DataItemsGenerator = new DataItemsGenerationConverter();
        private void OnAttachedObjectChanged(object oldValue, object newValue)
        {
            var oldValuePropertyNotificationChanged = oldValue as INotifyPropertyChanged;
            var oldValueCollectionNotificationChanged = oldValue as INotifyCollectionChanged;

            if (oldValuePropertyNotificationChanged != null)
                oldValuePropertyNotificationChanged.PropertyChanged -= OnAttachedObjectPropertyChanged;
       
            if (oldValueCollectionNotificationChanged != null && oldValue is IEnumerable enurable_oldValue)
            {
              if (enurable_oldValue is INotifyCollectionChanged notifyable_coll)
                    notifyable_coll.CollectionChanged -= OnAttachedObjectCollectionProperty_CollectionChanged;
              oldValueCollectionNotificationChanged.CollectionChanged -= OnAttachedObjectCollectionProperty_CollectionChanged;
            }

            var newValuePropertyNotificationChanged = newValue as INotifyPropertyChanged;
            var newValueCollectionNotificationChanged = newValue as INotifyCollectionChanged;

            if (newValuePropertyNotificationChanged != null)
            {
                DataItemsGenerator.Convert(this, null, null, CultureInfo.CurrentCulture);
                newValuePropertyNotificationChanged.PropertyChanged += OnAttachedObjectPropertyChanged;
            }
            if (newValueCollectionNotificationChanged != null && newValue is IEnumerable enurable_newValue)
            {

                this.Items = new DataItemCollection(this);
                foreach (IEntityObject obj in enurable_newValue)
                {
                    DataItem new_dataItem = new DataItem();
                    new_dataItem.AttachedObject = obj;
                    this.Items.Add(new_dataItem);
                }
                if (enurable_newValue is INotifyCollectionChanged notifyable_coll)
                {
                    notifyable_coll.CollectionChanged += OnAttachedObjectCollectionProperty_CollectionChanged;
                }

                newValueCollectionNotificationChanged.CollectionChanged += OnAttachedObjectCollectionProperty_CollectionChanged;
            }
        }


        public void OnAttachedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
      
        }

        private void OnAttachedObjectCollectionProperty_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntityObject obj in e.NewItems)
                {
                    if (!this.Items.Where(itm => itm.AttachedObject.StoredId == obj.StoredId).Any())
                    {
                        DataItem new_dataItem = new DataItem();
                        new_dataItem.AttachedObject = obj;
                        this.Items.Add(new_dataItem);
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IEntityObject obj in e.OldItems)
                {
                    DataItem old_dataItem = this.Items.Where(itm => itm.AttachedObject.StoredId == obj.StoredId).FirstOrDefault();
                    if (old_dataItem != null)
                        this.Items.Remove(old_dataItem);
                }
            }
        }
        /// <summary>
        /// ////////////////
        /// </summary>


        private bool _isHaveChanges;

        public bool IsHaveChanges
        {
            get { return _isHaveChanges; }
            set { _isHaveChanges = value; OnPropertyChanged("IsHaveChanges"); }
        }

        //private DataItemCollection _items;

        //public DataItemCollection Items
        //{
        //    get { return _items; }
        //    set { _items = value; }
        //}


        // public MenuItemExpandDelegateHandler MenuItemExpand;
        public DataItem()
        {
            //this._items = new DataItemCollection(this);

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


        //private object _attachedObject;
        //public object AttachedObject
        //{
        //    get { return _attachedObject; }
        //    set
        //    {
        //        _attachedObject = value;
        //        if (_attachedObject != null)
        //        {
        //            if (_attachedObject is INotifyPropertyChanged notifyable_object)
        //                notifyable_object.PropertyChanged += OnAttachedObjectPropertyChanged;
        //            if (_attachedObject is INotifyCollectionChanged notifyable_collection)
        //                notifyable_collection.CollectionChanged += OnAttachedCollectionChanged;
        //            if (_attachedObject is IList attached_collection)
        //            {
        //                foreach (object obj in attached_collection)
        //                {

        //                }
        //            }
        //            _allItems.Add(_attachedObject, this);
        //            OnAttachedObjectPropertyChanged(this, new PropertyChangedEventArgs("AttachedObject"));
        //        }
        //        OnPropertyChanged("AttachedObject");
        //        OnPropertyChanged("Type");
        //    }
        //}
        private void OnMenuItemFolded(DataItem dataItem)
        {

        }

        private void OnMenuItemExpand(DataItem dataItem)
        {

            //foreach (DataItem item in dataItem.Items)
            //{
            //    item.OnAttachedObjectPropertyChanged(item, new PropertyChangedEventArgs("IsExpanded"));
            //}
        }
        //public void OnAttachedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    //if (e.Action == NotifyCollectionChangedAction.Add &&  IsExpanded)
        //    //{
        //    //    foreach (object obj in e.NewItems)
        //    //    {
        //    //        if (Items.Where(itm => itm.AttachedObject == obj).FirstOrDefault() == null)
        //    //        {
        //    //            DataItem new_item = new DataItem();
        //    //            new_item.AttachedObject = obj;
        //    //       //     Items.Add(new_item);
        //    //        }
        //    //    }
        //    //}
        //    if (Parent != null && Parent.IsExpanded) { Parent.IsExpanded = false; }
        //    AttachedObjectCollectionChanged?.Invoke(this, sender, e);
        //    OnAttachedObjectPropertyChanged(this, new PropertyChangedEventArgs("AttachedCollectionChanged"));

        //}

       

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
        //private void OnSetAttachedObject()
        //{
        //    var prop_infoes = AttachedObject.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
        //    foreach (PropertyInfo prop_info in prop_infoes)
        //    {
        //        var prop_val = prop_info.GetValue(AttachedObject);
        //        if (!prop_info.PropertyType.FullName.Contains("System."))
        //        {
        //            if ((prop_val is IEntityObject))
        //            {
        //                DataItem dataItem = new DataItem();
        //                dataItem.PropName = prop_info.Name;
        //                dataItem.Type = prop_info.PropertyType;
        //                if (prop_val is IList list_prop)
        //                {
        //                    foreach (object obj in list_prop)
        //                    {
        //                        DataItem in_dataItem = new DataItem();
        //                        in_dataItem.PropName = prop_info.Name;
        //                        in_dataItem.Type = prop_info.PropertyType;
        //                    }
        //                }
        //                Items.Add(dataItem);



        //            }
        //        }
        //    }
        //}

        //private void OnExpand()
        //{
        //    //foreach (DataItem item in Items)
        //    //{
        //    //    IJornalable obj =(IJornalable) AttachedObject.GetType().GetProperty(item.PropName).GetValue(AttachedObject);
        //    //    if (obj != null) item.AttachedObject = obj;
        //    //}
        //}
    }
}
