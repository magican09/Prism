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
           
            base.OnApplyTemplate();

            BldTaskDataGridRow owningRow = DataGridRowOwner;
            owningRow.CellsPresenter = this;
            Item = owningRow.Item;
        }

        public object Item
        {
            get
            {
                return _item;
            }

            internal set
            {
                if (_item != value)
                {
                    object oldItem = _item;
                    _item = value;
                    OnItemChanged(oldItem, _item);
                }
            }
        }
        
        /// <summary>
        ///     Called when the value of the Item property changes.
        /// </summary>
        /// <param name="oldItem">The old value of Item.</param>
        /// <param name="newItem">The new value of Item.</param>
        protected virtual void OnItemChanged(object oldItem, object newItem)
        {
            ObservableCollection<object> columns = new ObservableCollection<object>();
            //ObservableCollection<DataGridColumn> columns = Columns;

            //if (columns != null)
            //{
            //    // Either update or create a collection that will return the row's data item
            //    // n number of times, where n is the number of columns.
            //    MultipleCopiesCollection cellItems = ItemsSource as MultipleCopiesCollection;
            //    if (cellItems == null)
            //    {
            //        cellItems = new MultipleCopiesCollection(newItem, columns.Count);
            //        ItemsSource = cellItems;
            //    }
            //    else
            //    {
            //        cellItems.CopiedItem = newItem;
            //    }
            //}
            TextBlock textBlock_1 = new TextBlock();
            textBlock_1.Text = "item 1";
            TextBlock textBlock_2 = new TextBlock();
            textBlock_2.Text = "item 2";
            TextBlock textBlock_3 = new TextBlock();
            textBlock_3.Text = "item 3";
            columns.Add(textBlock_1);
            columns.Add(textBlock_2);
            columns.Add(textBlock_3);

            ItemsSource = columns;
        }
        #region Helpers 
        internal BldTaskDataGridRow DataGridRowOwner
        {
            get { return DataGridHelper.FindParent<BldTaskDataGridRow>(this);  }
        }

        private ObservableCollection<BldTaskDataGridColumn> Columns
        {
            get
            {
                BldTaskDataGridRow owningRow = DataGridRowOwner;
                BldTaskDataGrid owningDataGrid = (owningRow != null) ? owningRow.DataGridOwner : null;
                return (owningDataGrid != null) ? owningDataGrid.Columns : null;
            }
        }
        #endregion
        #region Data

        private object _item;
     
        #endregion
    }
}
