using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Specialized;
using System.Windows;

namespace bldCustomControlLibrary
{
    public class DataGridExpandedItem : BindableBase
    {
        private Visibility _visible;
        public Visibility Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                SetProperty(ref _visible, value);
            }
        }

        private bool _isExpandable;
        public bool IsExpandable
        {
            get
            {
                return _isExpandable;
            }
            set
            {

                SetProperty(ref _isExpandable, value);
            }
        }
        private bool? _isExpanded;
        public bool? IsExpanded
        {
            get
            {
                if (Children.Count == 0)
                    _isExpanded = null;
                return _isExpanded;
            }
            set
            {
                if (value == true)
                    ExpandChildrenItems(this);
                else
                    CollapseChildrenItems(this);
                SetProperty(ref _isExpanded, value);
            }
        }

        private object _object;
        public object Object
        {
            get { return _object; }
            set { SetProperty(ref _object, value); }
        }
        private object _parent;
        public object Parent
        {
            get { return _parent; }
            set { SetProperty(ref _parent, value); }
        }
        private Thickness _nameMagrin;
        public Thickness NameMagrin
        {
            get { return _nameMagrin; }
            set { SetProperty(ref _nameMagrin, value); }
        }
        private DataGridExpandedItemCollection _children = new DataGridExpandedItemCollection();
        public DataGridExpandedItemCollection Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }
        public DataGridExpandedItem(object obj, bool? is_expanded)
        {
            Object = obj;
            IsExpanded = is_expanded;
            Children.CollectionChanged += OnChildrenChaged;

        }

        private void OnChildrenChaged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsExpandable = Children.Count > 0;
        }

        private void CollapseChildrenItems(DataGridExpandedItem e_obj)
        {
            foreach (DataGridExpandedItem expandedItem in e_obj.Children)
            {
                expandedItem.Visible = Visibility.Collapsed;
                CollapseChildrenItems(expandedItem);
            }
        }
        private void ExpandChildrenItems(DataGridExpandedItem e_obj)
        {
            foreach (DataGridExpandedItem expandedItem in e_obj.Children)
            {
                expandedItem.Visible = Visibility.Visible;
                ExpandChildrenItems(expandedItem);
            }
        }
    }
}
