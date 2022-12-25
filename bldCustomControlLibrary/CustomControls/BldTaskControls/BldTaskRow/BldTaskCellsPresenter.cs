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
    public class BldTaskCellsPresenter:ItemsControl
    {
        static  BldTaskCellsPresenter()
        {
          //  DefaultStyleKeyProperty.OverrideMetadata(typeof(BldTaskCellsPresenter), new FrameworkPropertyMetadata(typeof(BldTaskCellsPresenter)));

        }
        public ObservableCollection<BldTaskCell> cells_collection { get; set; }
        public bldWork work { get; set; }
        public BldTaskCellsPresenter()
        {
            cells_collection = new ObservableCollection<BldTaskCell>();
            BldTaskCell tsk_cell = new BldTaskCell();
            work = new bldWork();
            work.Name = "Work 1";
            tsk_cell.SetBinding(BldTaskCell.ContentProperty, new Binding("work") { Source = this });
            cells_collection.Add(tsk_cell);

            ItemsSource = cells_collection;
        }

        //
        // Summary:
        //     Gets the data item that the row represents.
        //
        // Returns:
        //     The data item that the row represents.
        public object Item { get; }


    
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
