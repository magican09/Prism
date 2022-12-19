using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace bldCustomControlLibrary
{
    public class ConstructionDataGrid: Control
    {

        //public bldWork DataSourse
        //{
        //    get { return (bldWork)GetValue(DataSourseProperty); }
        //    set { SetValue(DataSourseProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DataSourseProperty =
        //    DependencyProperty.Register("DataSourse", typeof(bldWork), typeof(ownerclass), new PropertyMetadata(0));

        public static RoutedEvent DataGridCell_MouseDoubleClickEvent =
            EventManager.RegisterRoutedEvent("DataGridCell_MouseDoubleClick", RoutingStrategy.Tunnel, typeof(RoutedEventArgs), typeof(ConstructionDataGrid));

        public static readonly RoutedCommand MouseDoubleClickRoutedCommand = new RoutedCommand();

        public event RoutedEventHandler MouseDoubleClick
        {
            add
            {
                AddHandler(DataGridCell_MouseDoubleClickEvent, value);
            }
            remove
            {
                RemoveHandler(DataGridCell_MouseDoubleClickEvent, value);
            }

        }
        private DataGrid _bldDataGrid;
        private DataGridExpandedItemCollection _dataGridExpandedItems = new DataGridExpandedItemCollection();


        static ConstructionDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConstructionDataGrid),
                new FrameworkPropertyMetadata(typeof(ConstructionDataGrid)));

        }
        public override void OnApplyTemplate()
        {

            _bldDataGrid = Template.FindName("bldWorkDataGrid", this) as DataGrid;
            _dataGridExpandedItems.Add(new DataGridExpandedItem(base.DataContext, true, true));
            _bldDataGrid.ItemsSource = _dataGridExpandedItems;
            _bldDataGrid.MouseDoubleClick += OnDataGridCell_MouseDoubleClick;
            base.OnApplyTemplate();
        }

        private void OnDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // get the row and column index based on the pointer position in WPF
            return;
            int numVisuals = VisualTreeHelper.GetChildrenCount(_bldDataGrid);
            DataGridRow dataGridRow = (DataGridRow)_bldDataGrid.ItemContainerGenerator.ContainerFromItem(_bldDataGrid.SelectedItem);
            var ff = _bldDataGrid.GetCell(_bldDataGrid.GetSelectedRow(), _bldDataGrid.CurrentCell.Column.DisplayIndex);
            for (int ii = 0; ii < numVisuals; ii++)
            {
                Visual v = (Visual)VisualTreeHelper.GetParent(_bldDataGrid);

            }
            var selected_cell = VisualTreeHelper.GetParent(_bldDataGrid.CurrentCell.Column);


            var rowColumnIndex = _bldDataGrid.GetCell(_bldDataGrid.GetSelectedRow(), _bldDataGrid.SelectedIndex);

            //Returns if caption summary or group summary row encountered.

            RaiseEvent(new RoutedEventArgs(DataGridCell_MouseDoubleClickEvent, this));
        }
        private void OnMouseDoubleClick(object sender, ExecutedRoutedEventArgs e)
        {
            // Do something
        }


    }
    public class DataGridExpandedItem : BindableBase
    {
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {

                SetProperty(ref _isExpanded, value);

            }
        }
        private bool _isChildrenExpanded;
        public bool IsChildrenExpanded
        {
            get { return _isChildrenExpanded; }
            set
            {
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
        public DataGridExpandedItem(object obj, bool is_expanded, bool is_children_expanded)
        {
            Object = obj;
            IsExpanded = is_expanded;
            IsChildrenExpanded = is_children_expanded;
        }
    }
    public class DataGridExpandedItemCollection : ObservableCollection<DataGridExpandedItem>
    {
        public DataGridExpandedItemCollection()
        {
            CollectionChanged += OnCollectionCahged;
        }

        private void OnCollectionCahged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DataGridExpandedItem elm in e.NewItems)
                {
                    if (elm.Object is bldConstruction construction)
                    {
                        foreach (bldWork work in construction.Works)
                        {
                            DataGridExpandedItem expandedItem = new DataGridExpandedItem(work, elm.IsChildrenExpanded, false);
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

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;
            // TODO: Your logic here
        }

    }
}
