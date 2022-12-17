using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class WorkDataRow:Control
    {


        //public bldWork DataSourse
        //{
        //    get { return (bldWork)GetValue(DataSourseProperty); }
        //    set { SetValue(DataSourseProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DataSourseProperty =
        //    DependencyProperty.Register("DataSourse", typeof(bldWork), typeof(ownerclass), new PropertyMetadata(0));


        private DataGrid _bldWorkDataGrid;
        private DataGridExpandedItemCollection _dataGridExpandedItems = new DataGridExpandedItemCollection();
        static WorkDataRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkDataRow), 
                new FrameworkPropertyMetadata(typeof(WorkDataRow)));

        }
        public override void OnApplyTemplate()
        {

            _bldWorkDataGrid = Template.FindName("bldWorkDataGrid",this) as DataGrid;
            
           // foreach (var element in base.DataContext as IEnumerable)
                _dataGridExpandedItems.Add(new DataGridExpandedItem(base.DataContext, true,true));

            _bldWorkDataGrid.ItemsSource = _dataGridExpandedItems;
            //_bldWorkDataGrid.RowStyle.

            //Binding is_expanded_binding = new Binding();
            
             base.OnApplyTemplate();
        }
    }
    public class DataGridExpandedItem : BindableBase
    {
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { 
             
                SetProperty(ref _isExpanded, value);
                
            }
        }
        private bool _isChildrenExpanded;
        public bool IsChildrenExpanded
        {
            get { return _isChildrenExpanded; }
            set {
                if (Children.Count > 0)
                    foreach (DataGridExpandedItem itm in Children)
                        itm.IsExpanded = value;
                else
                    _isChildrenExpanded = false;

                SetProperty(ref _isChildrenExpanded, value);
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
        public DataGridExpandedItem(object obj, bool is_expanded,bool is_children_expanded)
        {
            Object = obj;
            IsExpanded = is_expanded;
            IsChildrenExpanded = is_children_expanded;
        }
    }
        public class DataGridExpandedItemCollection:ObservableCollection<DataGridExpandedItem>
        {
        public DataGridExpandedItemCollection()
        {
            CollectionChanged += OnCollectionCahged;
        }

        private void OnCollectionCahged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action== NotifyCollectionChangedAction.Add)
            {
                foreach(DataGridExpandedItem elm in e.NewItems)
                {
                    if(elm.Object is bldConstruction construction)
                    {
                        foreach(bldWork work in construction.Works)
                        {
                            DataGridExpandedItem expandedItem = new DataGridExpandedItem(work, elm.IsChildrenExpanded,false);
                            expandedItem.Parent = elm;
                            this.Add(expandedItem);
                            elm.Children.Add(expandedItem);
                            expandedItem.NameMagrin = new Thickness(10, 0, 0, 0);

                            /// expandedItem.NameMagrin = new Thickness(10,0,0,0);


                        }
                    }
                }
                        
            }
        }
    }
    
}
