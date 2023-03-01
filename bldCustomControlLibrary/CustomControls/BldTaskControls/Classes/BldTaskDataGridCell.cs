using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace bldCustomControlLibrary
{
    public class BldTaskDataGridCell : DataGridCell
    {
        //public  DataGridColumn Column { get; }
        public BldTaskDataGridCell()
        {

        }
        #region Temlate 
        /// <summary>
        ///     Builds a column's visual tree if not using templates.
        /// </summary>
        internal void BuildVisualTree()
        {
            if (NeedsVisualTree)
            {
                var column = Column;
                if (column != null)
                {
                    // Work around a problem with BindingGroup not removing BindingExpressions.
                    //var row = RowOwner;
                    //if (row != null)
                    //{
                    //    var bindingGroup = row.BindingGroup;
                    //    if (bindingGroup != null)
                    //    {
                    //        RemoveBindingExpressions(bindingGroup, Content as DependencyObject);
                    //    }
                    //}

                    // Ask the column to build a visual tree
                    // FrameworkElement newContent = column.BuildVisualTree(IsEditing, RowDataItem, this);

                    // Before discarding the old visual tree, disconnect all its
                    // bindings, as in ItemContainerGenerator.UnlinkContainerFromItem.
                    // This prevents aliasing that can arise in recycling mode (DDVSO 405066)
                    //FrameworkElement oldContent = Content as FrameworkElement;
                    //if (oldContent != null && oldContent != newContent)
                    //{
                    //    ContentPresenter cp = oldContent as ContentPresenter;
                    //    if (cp == null)
                    //    {
                    //        oldContent.SetValue(FrameworkElement.DataContextProperty, BindingExpressionBase.DisconnectedItem);
                    //    }
                    //    else
                    //    {
                    //        // for a template column, disconnect by setting the
                    //        // Content, to override the binding set up in
                    //        // DataGridTemplateColumn.LoadTemplateContent.
                    //        cp.Content = BindingExpressionBase.DisconnectedItem;
                    //    }
                    //}

                    // hook the visual tree up through the Content property.
                    // Content = newContent;
                }
            }
        }
        #endregion
        #region Prepare Cell

        /// <summary>
        ///     Prepares a cell for use.
        /// </summary>
        /// <remarks>
        ///     Updates the column reference.
        ///     This overload computes the column index from the ItemContainerGenerator.
        /// </remarks>
        internal void PrepareCell(object item, ItemsControl cellsPresenter, BldTaskDataGridRow ownerRow)
        {
            PrepareCell(item, ownerRow, cellsPresenter.ItemContainerGenerator.IndexFromContainer(this));
        }
        /// <summary>
        ///     Prepares a cell for use.
        /// </summary>
        /// <remarks>
        ///     Updates the column reference.
        /// </remarks>
        internal void PrepareCell(object item, BldTaskDataGridRow ownerRow, int index)
        {
            Debug.Assert(_owner == null || _owner == ownerRow, "_owner should be null before PrepareCell is called or the same value as the ownerRow.");

            _owner = ownerRow;

            BldTaskDataGrid dataGrid = _owner.DataGridOwner;
            if (dataGrid != null)
            {
                // The index of the container should correspond to the index of the column
                if ((index >= 0) && (index < dataGrid.Columns.Count))
                {
                    // Retrieve the column definition and pass it to the cell container
                    DataGridColumn column = dataGrid.Columns[index];
                    Column = column;
                    TabIndex = column.DisplayIndex;
                }

                if (IsEditing)
                {
                    // If IsEditing was left on and this container was recycled, reset it here.
                    // Setting this property will result in BuildVisualTree being called.
                    IsEditing = false;
                }
                else if ((Content as FrameworkElement) == null)
                {
                    // If there isn't already a visual tree, then create one.
                    BuildVisualTree();

                    if (!NeedsVisualTree)
                    {
                        Content = item;
                    }
                }

                // Update cell Selection
                //  bool isSelected = dataGrid.SelectedCellsInternal.Contains(this);
                // SyncIsSelected(isSelected);
            }

            //DataGridHelper.TransferProperty(this, StyleProperty);
            //DataGridHelper.TransferProperty(this, IsReadOnlyProperty);
            CoerceValue(ClipProperty);
        }

        #endregion

        #region Column Information
        /// <summary>
        ///     The column that defines how this cell should appear.
        /// </summary>
        public DataGridColumn Column
        {
            get { return (DataGridColumn)GetValue(ColumnProperty); }
            internal set { SetValue(ColumnPropertyKey, value); }
        }

        /// <summary>
        ///     The DependencyPropertyKey that allows writing the Column property value.
        /// </summary>
        private static readonly DependencyPropertyKey ColumnPropertyKey =
            DependencyProperty.RegisterReadOnly("Column", typeof(DataGridColumn), typeof(BldTaskDataGridCell), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnColumnChanged)));

        /// <summary>
        ///     The DependencyProperty for the Columns property.
        /// </summary>
        public static readonly DependencyProperty ColumnProperty = ColumnPropertyKey.DependencyProperty;

        /// <summary>
        ///     Called when the Column property changes.
        ///     Calls the protected virtual OnColumnChanged.
        /// </summary>
        private static void OnColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BldTaskDataGridCell cell = sender as BldTaskDataGridCell;
            if (cell != null)
            {
                cell.OnColumnChanged((DataGridColumn)e.OldValue, (DataGridColumn)e.NewValue);
            }
        }

        #endregion
        #region Helpers 
        private bool NeedsVisualTree
        {
            get
            {
                return (ContentTemplate == null) && (ContentTemplateSelector == null);
            }
        }
        internal BldTaskDataGridRow RowOwner
        {
            get { return _owner; }
        }
        //private BldTaskDataGridCellsPresenter CellsPresenter
        //{
        //    get
        //    {
        //        return ItemsControl.ItemsControlFromItemContainer(this) as BldTaskDataGridCellsPresenter;
        //    }
        //}
        #endregion
        #region Data
        private BldTaskDataGridRow _owner;

        #endregion
    }
}
