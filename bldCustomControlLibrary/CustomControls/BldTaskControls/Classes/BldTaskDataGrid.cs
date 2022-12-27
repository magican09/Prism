using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        public BldTaskDataGrid()
        {
            ((INotifyCollectionChanged)Items).CollectionChanged += new NotifyCollectionChangedEventHandler(OnItemsCollectionChanged);


        }

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
            //TextBlock textBlock = new TextBlock();
            //textBlock.Text  = "nhjhnkj";
            //return textBlock;
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
            //if (row.DataGridOwner != this)
            //{
            //    //    row.Tracker.StartTracking(ref _rowTrackingRoot);
            //    if (item == CollectionView.NewItemPlaceholder ||
            //        (IsAddingNewItem && item == EditableItems.CurrentAddItem))
            //    {
            //        row.IsNewItem = true;
            //    }
            //    else
            //    {
            //        row.ClearValue(BldTaskDataGridRow.IsNewItemPropertyKey);
            //    }
            //    //   EnsureInternalScrollControls();
            //    //    EnqueueNewItemMarginComputation();
            //}
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

        private IEditableCollectionView EditableItems
        {
            get { return (IEditableCollectionView)Items; }
        }
        private bool IsAddingNewItem
        {
            get { return EditableItems.IsAddingNew; }
        }
        #endregion
        #region Colomn Autogeneration
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    if (DeferAutoGeneration)
            //    {
            //        // Add Auto columns only if it was deferred earlier
            //        AddAutoColumns();
            //    }
            //}
            //else if ((e.Action == NotifyCollectionChangedAction.Remove) || (e.Action == NotifyCollectionChangedAction.Replace))
            //{
            //    if (HasRowValidationError || HasCellValidationError)
            //    {
            //        foreach (object item in e.OldItems)
            //        {
            //            if (IsAddingOrEditingRowItem(item))
            //            {
            //                HasRowValidationError = false;
            //                HasCellValidationError = false;
            //                break;
            //            }
            //        }
            //    }
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Reset)
            //{
            //    ResetRowHeaderActualWidth();
            //    HasRowValidationError = false;
            //    HasCellValidationError = false;
            //}
        }
        #endregion
        #region Selection
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
          
            
        }
        #endregion
        #region Column Auto Generation 
        private static object OnCoerceItemsSourceProperty(DependencyObject d, object baseValue)
        {
            BldTaskDataGrid dataGrid = (BldTaskDataGrid)d;
            //if (baseValue != dataGrid._cachedItemsSource && dataGrid._cachedItemsSource != null)
            //{
            //    dataGrid.ClearSortDescriptionsOnItemsSourceChange();
            //}

            return baseValue;
        }
        /// <summary>
        ///     Helper method to clear SortDescriptions and all related
        ///     member when ItemsSource changes
        /// </summary>
        private void ClearSortDescriptionsOnItemsSourceChange()
        {
            Items.SortDescriptions.Clear();
            _sortingStarted = false;
            List<int> groupingSortDescriptionIndices = GroupingSortDescriptionIndices;
            if (groupingSortDescriptionIndices != null)
            {
                groupingSortDescriptionIndices.Clear();
            }
            //foreach (DataGridColumn column in Columns)
            //{
            //    column.SortDirection = null;
            //}
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

        private const string ItemsPanelPartName = "PART_RowsPresenter";
        #endregion
    }
}
