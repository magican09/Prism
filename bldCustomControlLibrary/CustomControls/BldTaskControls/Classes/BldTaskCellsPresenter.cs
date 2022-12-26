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
    public class BldTaskCellsPresenter: ItemsControl //DataGridCellsPresenter
    {
        static  BldTaskCellsPresenter()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(BldTaskCellsPresenter), new FrameworkPropertyMetadata(typeof(BldTaskCellsPresenter)));

        }
        public ObservableCollection<BldTaskCell> cells_collection { get; set; }
        public bldWork work { get; set; }
        public TextBlock textBlock { get; set; }
        public TextBlock textBlock_1 { get; set; }
        public TextBlock textBlock_2 { get; set; }
        public BldTaskCellsPresenter()
        {
            cells_collection = new ObservableCollection<BldTaskCell>();
            BldTaskCell tsk_cell = new BldTaskCell();
            BldTaskCell tsk_cell_1 = new BldTaskCell();
            BldTaskCell tsk_cell_2 = new BldTaskCell();
            work = new bldWork();
            work.Name = "Work 3";
            textBlock = new TextBlock();
            textBlock.Text = work.Name;
            textBlock_1 = new TextBlock();
            textBlock_1.Text = work.Name; 
            textBlock_2 = new TextBlock();
            textBlock_2.Text = work.Name;
            Item = work;
            tsk_cell.SetBinding(BldTaskCell.ContentProperty, new Binding("textBlock") { Source = this });
            tsk_cell_1.SetBinding(BldTaskCell.ContentProperty, new Binding("textBlock_1") { Source = this });
            tsk_cell_2.SetBinding(BldTaskCell.ContentProperty, new Binding("textBlock_2") { Source = this });
            cells_collection.Add(tsk_cell_1);
            cells_collection.Add(tsk_cell);
            cells_collection.Add(tsk_cell_2);
      
            this.SetBinding(BldTaskCellsPresenter.ItemsSourceProperty, new Binding("cells_collection") { Source = this });

           
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
