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
        private DataGridExpandedItemCollection _expandableDataItems = new DataGridExpandedItemCollection();


        static ConstructionDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConstructionDataGrid),
                new FrameworkPropertyMetadata(typeof(ConstructionDataGrid)));

        }
        public override void OnApplyTemplate()
        {

            _bldDataGrid = Template.FindName("bldWorkDataGrid", this) as DataGrid;
            DataGridExpandedItem _item = new DataGridExpandedItem(base.DataContext, true);
            _item.Visible = Visibility.Visible;
            _expandableDataItems.Add(_item);
            _bldDataGrid.ItemsSource = _expandableDataItems;
            base.OnApplyTemplate();
        }

        //private void OnDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    // get the row and column index based on the pointer position in WPF
        //    return;
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(_bldDataGrid);
        //    DataGridRow dataGridRow = (DataGridRow)_bldDataGrid.ItemContainerGenerator.ContainerFromItem(_bldDataGrid.SelectedItem);
        //    var ff = _bldDataGrid.GetCell(_bldDataGrid.GetSelectedRow(), _bldDataGrid.CurrentCell.Column.DisplayIndex);
        //    for (int ii = 0; ii < numVisuals; ii++)
        //    {
        //        Visual v = (Visual)VisualTreeHelper.GetParent(_bldDataGrid);

        //    }
        //    var selected_cell = VisualTreeHelper.GetParent(_bldDataGrid.CurrentCell.Column);


        //    var rowColumnIndex = _bldDataGrid.GetCell(_bldDataGrid.GetSelectedRow(), _bldDataGrid.SelectedIndex);

        //    //Returns if caption summary or group summary row encountered.

        //    RaiseEvent(new RoutedEventArgs(DataGridCell_MouseDoubleClickEvent, this));
        //}
     


    }
   
    
}
