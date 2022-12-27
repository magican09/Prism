using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace bldCustomControlLibrary
{ 
    public class BldTaskDataGridCellsPresenter: ItemsControl //DataGridCellsPresenter
    {
        static  BldTaskDataGridCellsPresenter()
        {
           DefaultStyleKeyProperty.OverrideMetadata(typeof(BldTaskDataGridCellsPresenter), new FrameworkPropertyMetadata(typeof(BldTaskDataGridCellsPresenter)));

        }
           
        public BldTaskDataGridCellsPresenter()
        {
          
        }
        public override void OnApplyTemplate()
        {
            ObservableCollection<object> collection = new ObservableCollection<object>();
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "CellPresenter";
            TextBlock textBlock2 = new TextBlock();
            textBlock2.Text = "CellPresenter 2";
            collection.Add(textBlock);
            collection.Add(textBlock2);
            ItemsSource = collection;
            base.OnApplyTemplate();
        }
        //protected override bool IsItemItsOwnContainerOverride(object item)
        //{
        //    return item is BldTaskCell;
        //}
        //protected override DependencyObject GetContainerForItemOverride()
        //{

        //    return new BldTaskCell();

        //}
        //
        // Summary:
        //     Gets the data item that the row represents.
        //
        // Returns:
        //     The data item that the row represents.
        public object Item { get; set; }


    
        // Summary:
        //     Updates the displayed cells when the System.Windows.Controls.Primitives.DataGridCellsPresenter.Item
        //     property value has changed.
        //
        // Parameters:
        //   oldItem:
        //     The previous value of the System.Windows.Controls.Primitives.DataGridCellsPresenter.Item
        //     property.
        //
        //   newItem:
        //     The new value of the System.Windows.Controls.Primitives.DataGridCellsPresenter.Item
        //     property.
        protected virtual void OnItemChanged(object oldItem, object newItem)
        {

        }
        
        // Summary:
        //     Updates the displayed cells when the System.Windows.Controls.DataGrid.Columns
        //     collection has changed.
        //
        // Parameters:
        //   columns:
        //     The System.Windows.Controls.DataGrid.Columns collection.
        //
        //   e:
        //     The event data from the System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged
        //     event of the System.Windows.Controls.DataGrid.Columns collection.
        protected internal virtual void OnColumnsChanged(ObservableCollection<DataGridColumn> columns, NotifyCollectionChangedEventArgs e)
        {

        }
    }
}
