﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace bldCustomControlLibrary
{
    public partial class BldTaskDataGrid : ItemsControl
    {
        static BldTaskDataGrid()
        {
            Type ownerType = typeof(BldTaskDataGrid);
            //  DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(typeof(BldTaskDataGrid)));
            ItemsSourceProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata((PropertyChangedCallback)null, OnCoerceItemsSourceProperty));


        }

        private static void OnCoerceItemsSourceProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        public BldTaskDataGrid()
        {
            _columns = new DataGridColumnCollection(this);
            _columns.CollectionChanged += new NotifyCollectionChangedEventHandler(OnColumnsChanged);

            ((INotifyCollectionChanged)Items).CollectionChanged += new NotifyCollectionChangedEventHandler(OnItemsCollectionChanged);
        }
        #region Templation
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion
        #region Item

        #endregion
        #region Notifications 
        internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {

        }
        #endregion
        #region Notification Propagation
       

        #endregion
        #region Row Generation
        /// <summary>
        ///     Determines if an item is its own container.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>true if the item is a DataGridRow, false otherwise.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BldTaskDataGridRow;
        }
        /// <summary>
        ///     Instantiates an instance of a container.
        /// </summary>
        /// <returns>A new DataGridRow.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BldTaskDataGridRow();
        }
        /// <summary>
        ///     Prepares a new container for a given item.
        /// </summary>
        /// <param name="element">The new container.</param>
        /// <param name="item">The item that the container represents.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            BldTaskDataGridRow row = (BldTaskDataGridRow)element;
            if (row.DataGridOwner != this)
            {
                row.Tracker.StartTracking(ref _rowTrackingRoot); //Создаем или регистриуем связаннный список DataRow  объектов
                if (item == CollectionView.NewItemPlaceholder ||
                    (IsAddingNewItem && item == EditableItems.CurrentAddItem)) //If DataRow added to DataGrid ItemsCollection 
                {
                    row.IsNewItem = true;
                }
                else
                {
                    row.ClearValue(BldTaskDataGridRow.IsNewItemPropertyKey);
                }
                //    //   EnsureInternalScrollControls();
                //    //    EnqueueNewItemMarginComputation();
            }
            row.PrepareRow(item, this);
            //    OnLoadingRow(new DataGridRowEventArgs(row));
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            DataGridRow row = (DataGridRow)element;
            //if (row.DataGridOwner == this)
            //{
            //    row.Tracker.StopTracking(ref _rowTrackingRoot);
            //    row.ClearValue(DataGridRow.IsNewItemPropertyKey);
            //    EnqueueNewItemMarginComputation();
            //}

            //OnUnloadingRow(new DataGridRowEventArgs(row));
            //row.ClearRow(this);
        }
     
        /// <summary>
        ///     Propagates the collection changed notification on Columns down to
        ///     each active DataGridRow.
        /// </summary>
        /// <param name="e">The event arguments from the original collection changed event.</param>
        private void UpdateColumnsOnRows(NotifyCollectionChangedEventArgs e)
        {
            ContainerTracking<BldTaskDataGridRow> tracker = _rowTrackingRoot;
            while (tracker != null)
            {
                tracker.Container.OnColumnsChanged(_columns, e);
                tracker = tracker.Next;
            }
        }
        private IEditableCollectionView EditableItems
        {
            get { return (IEditableCollectionView)Items; }
        }
        private bool IsAddingNewItem
        {
            get { return EditableItems.IsAddingNew; }
        }
        #endregion
        #region Columns Notification

       

        #endregion
        //#region Colomn Autogeneration
        //protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    base.OnItemsChanged(e);

        //    //if (e.Action == NotifyCollectionChangedAction.Add)
        //    //{
        //    //    if (DeferAutoGeneration)
        //    //    {
        //    //        // Add Auto columns only if it was deferred earlier
        //    //        AddAutoColumns();
        //    //    }
        //    //}
        //    //else if ((e.Action == NotifyCollectionChangedAction.Remove) || (e.Action == NotifyCollectionChangedAction.Replace))
        //    //{
        //    //    if (HasRowValidationError || HasCellValidationError)
        //    //    {
        //    //        foreach (object item in e.OldItems)
        //    //        {
        //    //            if (IsAddingOrEditingRowItem(item))
        //    //            {
        //    //                HasRowValidationError = false;
        //    //                HasCellValidationError = false;
        //    //                break;
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //else if (e.Action == NotifyCollectionChangedAction.Reset)
        //    //{
        //    //    ResetRowHeaderActualWidth();
        //    //    HasRowValidationError = false;
        //    //    HasCellValidationError = false;
        //    //}
        //}
        //#endregion
        #region Selection
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {


        }
        #endregion
        #region Column (Auto) Generation 
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            // ItemsControl calls a ClearValue on ItemsSource property
            // whenever it is set to null. So Coercion is not called
            // in such case. Hence clearing the SortDescriptions and
            // GroupDescriptions here when new value is null.
            //if (newValue == null)
            //{
            //    ClearSortDescriptionsOnItemsSourceChange();
            //}

            _cachedItemsSource = newValue;

            //using (UpdateSelectedCells())
            //{
            //    // Selector will try to maintain the previous row selection.
            //    // Keep SelectedCells in sync.
            //    List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();
            //    LocateSelectedItems(ranges);
            //    _selectedCells.RestoreOnlyFullRows(ranges);
            //}

             //if (AutoGenerateColumns == true)
             if(false)
            {
                RegenerateAutoColumns();
            }

            //InternalColumns.RefreshAutoWidthColumns = true;
            //InternalColumns.InvalidateColumnWidthsComputation();

            //CoerceValue(CanUserAddRowsProperty);
            //CoerceValue(CanUserDeleteRowsProperty);
            //DataGridHelper.TransferProperty(this, CanUserSortColumnsProperty);

            //ResetRowHeaderActualWidth();

            //UpdateNewItemPlaceholder(/* isAddingNewItem = */ false);

            //HasCellValidationError = false;
            //HasRowValidationError = false;
        }

        /// <summary>
        /// Method which regenerates the columns for the datagrid
        /// </summary>
        private void RegenerateAutoColumns()
        {
            //    DeleteAutoColumns();
            AddColumns();
        }

        /// <summary>
        /// Method which generated auto columns and adds to the data grid.
        /// </summary>
        private void AddColumns()
        {
            ReadOnlyCollection<ItemPropertyInfo> itemProperties = ((IItemProperties)Items).ItemProperties;

            //if (itemProperties == null && DataItemsCount == 0)
            //{
            //    // do deferred generation
            //    DeferAutoGeneration = true;
            //}
            //else if (!_measureNeverInvoked)
            //{
            //    DataGrid.GenerateColumns(
            //        itemProperties,
            //        this,
            //        null);

            //    DeferAutoGeneration = false;

            //    OnAutoGeneratedColumns(EventArgs.Empty);
            //}

            BldTaskDataGrid.GenerateColumns(
                  itemProperties,
                  this,
                  null);
        }
        /// <summary>
        /// Helper method which generates columns for a given IItemProperties
        /// </summary>
        /// <param name="iItemProperties"></param>
        /// <returns></returns>
        public static Collection<BldTaskDataGridColumn> GenerateColumns(IItemProperties itemProperties)
        {
            if (itemProperties == null)
            {
                throw new ArgumentNullException("itemProperties");
            }

            Collection<BldTaskDataGridColumn> columnCollection = new Collection<BldTaskDataGridColumn>();
            BldTaskDataGrid.GenerateColumns(
                itemProperties.ItemProperties,
                null,
                columnCollection);
            return columnCollection;
        }

        /// <summary>
        /// Helper method which generates columns for a given IItemProperties and adds
        /// them either to a datagrid or to a collection of columns as specified by the flag.
        /// </summary>
        /// <param name="itemProperties"></param>
        /// <param name="dataGrid"></param>
        /// <param name="columnCollection"></param>
        private static void GenerateColumns(
            ReadOnlyCollection<ItemPropertyInfo> itemProperties,
            BldTaskDataGrid dataGrid,
            Collection<BldTaskDataGridColumn> columnCollection)
        {
            Debug.Assert(dataGrid != null || columnCollection != null, "Both dataGrid and columnCollection cannot not be null at the same time");

            if (itemProperties != null &&
                itemProperties.Count > 0)
            {
                foreach (ItemPropertyInfo itemProperty in itemProperties)
                {
                    BldTaskDataGridColumn dataGridColumn = BldTaskDataGridColumn.CreateDefaultColumn(itemProperty);

                    if (dataGrid != null)
                    {
                        // AutoGeneratingColumn event is raised before generating and adding column to datagrid
                        // and the column returned by the event handler is used instead of the original column.
                        //DataGridAutoGeneratingColumnEventArgs eventArgs = new DataGridAutoGeneratingColumnEventArgs(dataGridColumn, itemProperty);
                        //dataGrid.OnAutoGeneratingColumn(eventArgs);

                        //if (!eventArgs.Cancel && eventArgs.Column != null)
                        //{
                        //    eventArgs.Column.IsAutoGenerated = true;
                        //    dataGrid.Columns.Add(eventArgs.Column);
                        //}
                        dataGrid.Columns.Add(dataGridColumn);
                    }
                    else
                    {
                        columnCollection.Add(dataGridColumn);
                    }

                }
            }
        }

        /// <summary>
        /// The polymorphic method which gets called whenever the ItemsSource gets changed.
        /// We regenerate columns if required when ItemsSource gets changed.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        #endregion
        #region Columns

        /// <summary>
        ///     A collection of column definitions describing the individual
        ///     columns of each row.
        /// </summary>
        public ObservableCollection<BldTaskDataGridColumn> Columns
        {
            get { return _columns; }
        }
        /// <summary>
        ///     Updates the reference to this DataGrid on the list of columns.
        /// </summary>
        /// <param name="list">The list of affected columns.</param>
        /// <param name="clear">Whether to add or remove the reference to this grid.</param>
        internal void UpdateDataGridReference(IList list, bool clear)
        {
            int numItems = list.Count;
            for (int i = 0; i < numItems; i++)
            {
                BldTaskDataGridColumn column = (BldTaskDataGridColumn)list[i];
                if (clear)
                {
                    // Set the owner to null only if the current owner is this grid
                    if (column.DataGridOwner == this)
                    {
                        column.DataGridOwner = null;
                    }
                }
                else
                {
                    // Remove the column from any old owner
                    if (column.DataGridOwner != null && column.DataGridOwner != this)
                    {
                        column.DataGridOwner.Columns.Remove(column);
                    }

                    column.DataGridOwner = this;
                }
            }
        }
        /// <summary>
        ///     Called when the Columns collection changes.
        /// </summary>
        private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update the reference to this DataGrid on the affected column(s)
            // and update the SelectedCells collection.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateDataGridReference(e.NewItems, /* clear = */ false);
                    UpdateColumnSizeConstraints(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    UpdateDataGridReference(e.OldItems, /* clear = */ true);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    UpdateDataGridReference(e.OldItems, /* clear = */ true);
                    UpdateDataGridReference(e.NewItems, /* clear = */ false);
                    UpdateColumnSizeConstraints(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    // We can't clear column references on Reset: _columns has 0 items and e.OldItems is empty.
                  //  _selectedCells.Clear();
                    break;
            }

            //// FrozenColumns rely on column DisplayIndex
            //// Delay the coercion if necessary
            //if (InternalColumns.DisplayIndexMapInitialized)
            //{
            //    CoerceValue(FrozenColumnCountProperty);
            //}

            //bool visibleColumnsChanged = HasVisibleColumns(e.OldItems);
            //visibleColumnsChanged |= HasVisibleColumns(e.NewItems);
            //visibleColumnsChanged |= (e.Action == NotifyCollectionChangedAction.Reset);

            //if (visibleColumnsChanged)
            //{
            //    InternalColumns.InvalidateColumnRealization(true);
            //}

            UpdateColumnsOnRows(e);

            //// Recompute the column width if required, but wait until the first load
            //if (visibleColumnsChanged && e.Action != NotifyCollectionChangedAction.Move)
            //{
            //    InternalColumns.InvalidateColumnWidthsComputation();
            //}
        }

        /// <summary>
        ///     Updates the transferred size constraints from DataGrid on the columns.
        /// </summary>
        /// <param name="list">The list of affected columns.</param>
        private static void UpdateColumnSizeConstraints(IList list)
        {
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                var column = (BldTaskDataGridColumn)list[i];
                column.SyncProperties();
            }
        }
        #endregion
        #region Auto Sort

        /// <summary>
        /// List which holds all the indices of SortDescriptions which were
        /// added for the sake of GroupDescriptions
        /// </summary>
        private List<int> GroupingSortDescriptionIndices
        {
            get
            {
                return _groupingSortDescriptionIndices;
            }

            set
            {
                _groupingSortDescriptionIndices = value;
            }
        }
        #endregion
        #region Helpers

        #endregion
        #region Data
        private IEnumerable _cachedItemsSource = null;                      // Reference to the ItemsSource instance, used to clear SortDescriptions on ItemsSource change
        private bool _sortingStarted = false;                               // Flag used to track if Sorting ever started or not.
        private List<int> _groupingSortDescriptionIndices = null;           // List to hold the indices of SortDescriptions added for the sake of GroupDescriptions.
        private DataGridCell _currentCellContainer;                         // Reference to the cell container corresponding to CurrentCell (use CurrentCellContainer property instead)
        private ObservableCollection<BldTaskDataGridColumn> _columns ;                          // Stores the columns

        private ContainerTracking<BldTaskDataGridRow> _rowTrackingRoot;            // Root of a linked list of active row containers


        private const string ItemsPanelPartName = "PART_RowsPresenter";
        #endregion
    }
}
