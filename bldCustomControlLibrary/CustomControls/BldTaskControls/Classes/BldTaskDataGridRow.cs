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

namespace bldCustomControlLibrary
{
    public class BldTaskDataGridRow: Control
    {
        static  BldTaskDataGridRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BldTaskDataGridRow), new FrameworkPropertyMetadata(typeof(BldTaskDataGridRow)));

        }
        public BldTaskDataGridRow()
        {
            _tracker = new ContainerTracking<BldTaskDataGridRow>(this);
        }
        public override void OnApplyTemplate()
        {
            //      CellsPresenter = Template.FindName("PART_BldTaskDataGridCellsPresenter",this) as BldTaskDataGridCellsPresenter;
          base.OnApplyTemplate();
           
        }
        #region Data Item

        /// <summary>
        ///     The item that the row represents. This item is an entry in the list of items from the DataGrid.
        ///     From this item, cells are generated for each column in the DataGrid.
        /// </summary>
        public object Item
        {
            get { return GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the Item property.
        /// </summary>
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(object), typeof(BldTaskDataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnNotifyRowPropertyChanged)));

        private static void OnNotifyRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BldTaskDataGridRow).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Rows);
        }
        internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            NotifyPropertyChanged(d, string.Empty, e, target);
        }
        internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            if (DataGridHelper.ShouldNotifyRows(target))
            {
                if (e.Property == ItemProperty)
                {
                    OnItemChanged(e.OldValue, e.NewValue);
                }
               
            }

        }
        protected virtual void OnItemChanged(object oldItem, object newItem)
        {
           BldTaskDataGridCellsPresenter cellsPresenter = CellsPresenter;
            if (cellsPresenter != null)
            {
                cellsPresenter.Item = newItem;
            }
        }

        #endregion
        #region Columns Notification
        /// <summary>
        ///     Set by the CellsPresenter when it is created.  Used by the Row to send down property change notifications.
        /// </summary>
        internal BldTaskDataGridCellsPresenter CellsPresenter
        {
            get { return _cellsPresenter; }
            set { _cellsPresenter = value; }
        }
        #endregion
        #region Notification Propagation

        /// <summary>
        ///     Notification from the DataGrid that the columns collection has changed.
        /// </summary>
        /// <param name="columns">The columns collection.</param>
        /// <param name="e">The event arguments from the collection's change event.</param>
        protected internal virtual void OnColumnsChanged(ObservableCollection<BldTaskDataGridColumn> columns, NotifyCollectionChangedEventArgs e)
        {
            BldTaskDataGridCellsPresenter cellsPresenter = CellsPresenter;
            if (cellsPresenter != null)
            {
                cellsPresenter.OnColumnsChanged(columns, e);
            }
        }
        #endregion
        #region Row Generation

        /// <summary>
        ///     Fired when the Row is attached to the DataGrid.  The scenario here is if the user is scrolling and
        ///     the Row is a recycled container that was just added back to the visual tree.  Properties that rely on a value from
        ///     the Grid should be reevaluated because they may be stale.  
        /// </summary>
        /// <remarks>
        ///     Properties can obviously be stale if the DataGrid's value changes while the row is disconnected.  They can also
        ///     be stale for unobvious reasons.
        /// 
        ///     For example, the Style property is invalidated when we detect a new Visual parent.  This happens for 
        ///     elements in the row (such as the RowHeader) before Prepare is called on the Row.  The coercion callback
        ///     will thus be unable to find the DataGrid and will return the wrong value.  
        /// 
        ///     There is a potential for perf work here.  If we know a DP isn't invalidated when the visual tree is reconnected
        ///     and we know that the Grid hasn't modified that property then its value is likely fine.  We could also cache whether
        ///     or not the Grid's property is the one that's winning.  If not, no need to redo the coercion.  This notification 
        ///     is pretty fast already and thus not worth the work for now.
        /// </remarks>
        private void SyncProperties(bool forcePrepareCells)
        {
            // Coerce all properties on Row that depend on values from the DataGrid
            // Style is ok since it's equivalent to ItemContainerStyle and has already been invalidated.
            DataGridHelper.TransferProperty(this, BackgroundProperty);
            //DataGridHelper.TransferProperty(this, HeaderStyleProperty);
            //DataGridHelper.TransferProperty(this, HeaderTemplateProperty);
            //DataGridHelper.TransferProperty(this, HeaderTemplateSelectorProperty);
            //DataGridHelper.TransferProperty(this, ValidationErrorTemplateProperty);
            //DataGridHelper.TransferProperty(this, DetailsTemplateProperty);
            //DataGridHelper.TransferProperty(this, DetailsTemplateSelectorProperty);
            //DataGridHelper.TransferProperty(this, DetailsVisibilityProperty);

            CoerceValue(VisibilityProperty); // Handle NewItemPlaceholder case

            //RestoreAttachedItemValue(this, DetailsVisibilityProperty);

            var cellsPresenter = CellsPresenter;
            if (cellsPresenter != null)
            {
                //cellsPresenter.SyncProperties(forcePrepareCells);
                //RestoreAttachedItemValue(cellsPresenter, DataGridCellsPresenter.HeightProperty);
            }

            //if (DetailsPresenter != null)
            //{
            //    DetailsPresenter.SyncProperties();
            //}

            //if (RowHeader != null)
            //{
            //    RowHeader.SyncProperties();
            //}
        }

        /// <summary>
        ///     Prepares a row container for active use.
        /// </summary>
        /// <remarks>
        ///     Instantiates or updates a MultipleCopiesCollection ItemsSource in
        ///     order that cells be generated.
        /// </remarks>
        /// <param name="item">The data item that the row represents.</param>
        /// <param name="owningDataGrid">The DataGrid owner.</param>
        internal void PrepareRow(object item, BldTaskDataGrid owningDataGrid)
        {
            bool fireOwnerChanged = (_owner != owningDataGrid);
            Debug.Assert(_owner == null || _owner == owningDataGrid, "_owner should be null before PrepareRow is called or the same as the owningDataGrid.");
            bool forcePrepareCells = false;
            _owner = owningDataGrid;

            if (this != item)
            {
                if (Item != item)
                {
                    Item = item;
                }
                else
                {
                    forcePrepareCells = true;
                }

                
                // Since we just changed _owner we need to invalidate all child properties that rely on a value supplied by the DataGrid.
                // A common scenario is when a recycled Row was detached from the visual tree and has just been reattached (we always clear out the 
                // owner when recycling a container).
                if (fireOwnerChanged)
                {
                    SyncProperties(forcePrepareCells);
                }
            }

            //if (IsEditing)
            //{
            //    // If IsEditing was left on and this container was recycled, reset it here.
            //    IsEditing = false;
            //}

            //// Since we just changed _owner we need to invalidate all child properties that rely on a value supplied by the DataGrid.
            //// A common scenario is when a recycled Row was detached from the visual tree and has just been reattached (we always clear out the
            //// owner when recycling a container).
            //if (fireOwnerChanged)
            //{
            //    SyncProperties(forcePrepareCells);
            //}

            //CoerceValue(VirtualizingPanel.ShouldCacheContainerSizeProperty);

            //// Re-run validation, but wait until Binding has occured.
            //Dispatcher.BeginInvoke(new DispatcherOperationCallback(DelayedValidateWithoutUpdate), DispatcherPriority.DataBind, BindingGroup);
        }
        /// <summary>
        ///     Used by the DataGrid owner to send notifications to the row container.
        /// </summary>
        internal ContainerTracking<BldTaskDataGridRow> Tracker
        {
            get { return _tracker; }
        }

        #endregion
        #region Helpers
        /// <summary>
        ///     Returns the index of this row within the DataGrid's list of item containers.
        /// </summary>
        /// <remarks>
        ///     This method performs a linear search.
        /// </remarks>
        /// <returns>The index, if found, -1 otherwise.</returns>
        public int GetIndex()
        {
            BldTaskDataGrid dataGridOwner = DataGridOwner;
            if (dataGridOwner != null)
            {
                return dataGridOwner.ItemContainerGenerator.IndexFromContainer(this);
            }

            return -1;
        }
        public BldTaskDataGrid DataGridOwner
        {
            get { return _owner; }
        }
        #endregion
        #region Template
      
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            CellsPresenter = null;
         //   DetailsPresenter = null;
        }
        /// <summary>
        ///     A template that will generate the panel that arranges the cells in this row.
        /// </summary>
        /// <remarks>
        ///     The template for the row should contain an ItemsControl that template binds to this property.
        /// </remarks>
        //public ItemsPanelTemplate ItemsPanel
        //{
        //    get { return (ItemsPanelTemplate)GetValue(ItemsPanelProperty); }
        //    set { SetValue(ItemsPanelProperty, value); }
        //}

        /// <summary>
        ///     The DependencyProperty that represents the ItemsPanel property.
        /// </summary>
       // public static readonly DependencyProperty ItemsPanelProperty = ItemsControl.ItemsPanelProperty.AddOwner(typeof(DataGridRow));
        #endregion
        #region New Item

        /// <summary>
        ///     Indicates whether the row belongs to new item (both placeholder
        ///     as well as adding item) or not.
        /// </summary>
        public bool IsNewItem
        {
            get { return (bool)GetValue(IsNewItemProperty); }
            internal set { SetValue(IsNewItemPropertyKey, value); }
        }

        /// <summary>
        ///     Using a DependencyProperty as the backing store for IsNewItem.  This enables animation, styling, binding, etc...
        /// </summary>
        internal static readonly DependencyPropertyKey IsNewItemPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNewItem", typeof(bool), typeof(BldTaskDataGridRow), new FrameworkPropertyMetadata(false));

        /// <summary>
        ///     DependencyProperty for IsNewItem property.
        /// </summary>
        public static readonly DependencyProperty IsNewItemProperty =
            IsNewItemPropertyKey.DependencyProperty;

        #endregion
        #region Data
        private BldTaskDataGrid _owner;
        private BldTaskDataGridCellsPresenter _cellsPresenter;
        private ContainerTracking<BldTaskDataGridRow> _tracker;
        #endregion
    }
}
