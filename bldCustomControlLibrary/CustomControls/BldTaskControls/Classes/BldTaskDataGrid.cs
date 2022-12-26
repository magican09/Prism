using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace bldCustomControlLibrary
{
    public partial class BldTaskDataGrid:DataGrid
    {
        public BldTaskDataGrid()
        {
            Type ownerType = typeof(BldTaskDataGrid);

            DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(typeof(DataGrid)));
            FrameworkElementFactory dataGridRowPresenterFactory = new FrameworkElementFactory(typeof(DataGridRowsPresenter));
            dataGridRowPresenterFactory.SetValue(FrameworkElement.NameProperty, ItemsPanelPartName);
            ItemsPanelProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(
                new ItemsPanelTemplate(dataGridRowPresenterFactory)));
            ItemContainerStyleProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceItemContainerStyle)));
            ItemContainerStyleSelectorProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceItemContainerStyleSelector)));
            ItemsSourceProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata((PropertyChangedCallback)null, OnCoerceItemsSourceProperty));

            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Заголово!";
            Columns.Add(textColumn);
            
        }

        private void OnCoerceItemsSourceProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
         
        }

        private bool OnValiItemContainerStyle(object value)
        {
            throw new NotImplementedException();
        }

        private object OnCoerceItemContainerStyleSelector(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private object OnCoerceItemContainerStyle(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private object OnCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }
        #region Data
        private const string ItemsPanelPartName = "PART_RowsPresenter";
        #endregion
    }
}
