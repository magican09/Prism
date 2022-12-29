using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
            ObservableCollection<object> collection = new ObservableCollection<object>();
            ObservableCollection<BldTaskDataGridColumn> columns = Columns;

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
            collection.Add(textBlock_1);
            collection.Add(textBlock_2);
            collection.Add(textBlock_3);

            ItemsSource = collection;
        }

        #region Row Communication
       /// <summary>
        ///     Tells the row owner about this element.
        /// </summary>
        public override void OnApplyTemplate()
        {
            // If a new template has just been generated then 
            // be sure to clear any stale ItemsHost references
            //if (InternalItemsHost != null && !this.IsAncestorOf(InternalItemsHost))
            //{
            //    InternalItemsHost = null;
            //}

            base.OnApplyTemplate();

            BldTaskDataGridRow owningRow = DataGridRowOwner;
            owningRow.CellsPresenter = this;
            Item = owningRow.Item;

            // At the time that a Row is prepared we can't Sync because the CellsPresenter isn't created yet.
            // Doing it here ensures that the CellsPresenter is in the visual tree.
            SyncProperties(false);

        }
        internal void SyncProperties(bool forcePrepareCells)
        {
            var dataGridOwner = DataGridOwner;
            if (dataGridOwner == null)
            {
                return;
            }

            //DataGridHelper.TransferProperty(this, HeightProperty);
            //DataGridHelper.TransferProperty(this, MinHeightProperty);
            //DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);

            // This is a convenient way to walk through all cells and force them to call CoerceValue(StyleProperty)
            //NotifyPropertyChanged(this, new DependencyPropertyChangedEventArgs(DataGrid.CellStyleProperty, null, null), DataGridNotificationTarget.Cells);

            // We may have missed an Add / Remove of a column from the grid (DataGridRow.OnColumnsChanged)
            // Sync the MultipleCopiesCollection count and update the Column on changed cells
            //MultipleCopiesCollection cellItems = ItemsSource as MultipleCopiesCollection;
            ObservableCollection<object> cellItems = ItemsSource as ObservableCollection<object>;
            if (cellItems != null)
            {
                BldTaskDataGridCell cell;
                ObservableCollection<BldTaskDataGridColumn> columns = dataGridOwner.Columns;
                int newColumnCount = columns.Count;
                int oldColumnCount = cellItems.Count;
                int dirtyCount = 0;
                bool measureAndArrangeInvalidated = false;

                //if (newColumnCount != oldColumnCount)
                //{
                //    cellItems.SyncToCount(newColumnCount);

                //    // Newly added or removed containers will be updated by the generator via PrepareContainer.
                //    // All others may have a different column
                //    dirtyCount = Math.Min(newColumnCount, oldColumnCount);
                //}
                //else if (forcePrepareCells)
                //{
                //    dirtyCount = newColumnCount;
                //}

                // if the DataGridCellsPanel missed out on some column virtualization
                // activity while the row was virtualized, it needs to be measured
                DataGridCellsPanel cellsPanel = InternalItemsHost as DataGridCellsPanel;
                if (cellsPanel != null)
                {
                    //if (cellsPanel.HasCorrectRealizedColumns)
                    //{
                    //    // This operation is performed when a DataGridRow is being prepared. So if we are working 
                    //    // with a recycled DataGridRow we need to make sure to re-arrange it so that it picks up the 
                    //    // correct CellsPanelHorizontalOffset. See Dev11 170908.
                    //    cellsPanel.InvalidateArrange();
                    //}
                    //else
                    //{
                    //    InvalidateDataGridCellsPanelMeasureAndArrange();
                    //    measureAndArrangeInvalidated = true;
                    //}
                }

                BldTaskDataGridRow row = DataGridRowOwner;

                //// Prepare the cells until dirtyCount is reached. Also invalidate the cells panel's measure
                //// and arrange if there is a mismatch between cell.ActualWidth and Column.Width.DisplayValue
                //for (int i = 0; i < dirtyCount; i++)
                //{
                //    cell = (BldTaskDataGridCell)ItemContainerGenerator.ContainerFromIndex(i);
                //    if (cell != null)
                //    {
                //        cell.PrepareCell(row.Item, this, row);
                //        if (!measureAndArrangeInvalidated && !DoubleUtil.AreClose(cell.ActualWidth, columns[i].Width.DisplayValue))
                //        {
                //            InvalidateDataGridCellsPanelMeasureAndArrange();
                //            measureAndArrangeInvalidated = true;
                //        }
                //    }
                //}

                // Keep searching for the mismatch between cell.ActualWidth
                // and Column.Width.DisplayValue
                //if (!measureAndArrangeInvalidated)
                //{
                //    for (int i = dirtyCount; i < newColumnCount; i++)
                //    {
                //        cell = (DataGridCell)ItemContainerGenerator.ContainerFromIndex(i);
                //        if (cell != null)
                //        {
                //            if (!DoubleUtil.AreClose(cell.ActualWidth, columns[i].Width.DisplayValue))
                //            {
                //                InvalidateDataGridCellsPanelMeasureAndArrange();
                //                break;
                //            }
                //        }
                //    }
                //}
            }
        }
        #endregion
        #region Cell Container Genertor
      
        /// <summary>
        ///     Workaround for not being able to access the panel instance of
        ///     itemscontrol directly
        /// </summary>
        internal Panel InternalItemsHost
        {
            get { return _internalItemsHost; }
            set { _internalItemsHost = value; }
        }

        /// <summary>
        ///     Notification from the DataGrid that the columns collection has changed.
        /// </summary>
        /// <param name="columns">The columns collection.</param>
        /// <param name="e">The event arguments from the collection's change event.</param>
        protected internal virtual void OnColumnsChanged(ObservableCollection<BldTaskDataGridColumn> columns, NotifyCollectionChangedEventArgs e)
        {
            // Update the ItemsSource for the cells
            //MultipleCopiesCollection cellItems = ItemsSource as MultipleCopiesCollection;
            //if (cellItems != null)
            //{
            //    cellItems.MirrorCollectionChange(e);
            //}
            var itms = ItemsSource;
            // For a reset event the only thing the MultipleCopiesCollection can do is set its count to 0.
            Debug.Assert(
                e.Action != NotifyCollectionChangedAction.Reset || columns.Count == 0,
                "A Reset event should only be fired for a Clear event from the columns collection");
        }
        #endregion
        #region Helpers 
        /// <summary>
        ///     The DataGrid that owns this control
        /// </summary>
        internal BldTaskDataGrid DataGridOwner
        {
            get
            {
                BldTaskDataGridRow parent = DataGridRowOwner;
                if (parent != null)
                {
                    return parent.DataGridOwner;
                }

                return null;
            }
        }
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
        private Panel _internalItemsHost;

        #endregion
    }
}
