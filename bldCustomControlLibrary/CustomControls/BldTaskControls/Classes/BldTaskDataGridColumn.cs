using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace bldCustomControlLibrary 
{
    public abstract partial class BldTaskDataGridColumn : DataGridBoundColumn
    {

        #region Oner Communication
        protected internal DataGrid DataGridOwner
        {
            get { return _dataGridOwner; }
            internal set { _dataGridOwner = value; }
        }

        #endregion
        #region Hidden Columns
        /// <summary>
        ///     Helper IsVisible property
        /// </summary>
        internal bool IsVisible
        {
            get
            {
                return Visibility == Visibility.Visible;
            }
        }
        #endregion
        #region Autogeneration
        internal static BldTaskDataGridColumn CreateDefaultColumn(ItemPropertyInfo itemProperty)
        {
            Debug.Assert(itemProperty != null && itemProperty.PropertyType != null, "itemProperty and/or its PropertyType member cannot be null");

           // DataGridColumn dataGridColumn = null;
            BldTaskDataGridColumn dataGridColumn = null;

            DataGridComboBoxColumn comboBoxColumn = null;
            Type propertyType = itemProperty.PropertyType;
            
            // determine the type of column to be created and create one
            //if (propertyType.IsEnum)
            //{
            //    comboBoxColumn = new DataGridComboBoxColumn();
            //    comboBoxColumn.ItemsSource = Enum.GetValues(propertyType);
            //    dataGridColumn = comboBoxColumn as DataGridColumn;
            //}
            //else if (typeof(string).IsAssignableFrom(propertyType))
            //{
            //    dataGridColumn = new DataGridTextColumn();
            //}
            //else if (typeof(bool).IsAssignableFrom(propertyType))
            //{
            //    dataGridColumn = new DataGridCheckBoxColumn();
            //}
            //else if (typeof(Uri).IsAssignableFrom(propertyType))
            //{
            //    dataGridColumn = new DataGridHyperlinkColumn();
            //}
            //else
            //{
            //    dataGridColumn = new DataGridTextColumn();
            //}
            dataGridColumn = new BldTaskDataGridTextColumn();
            // determine if the datagrid can sort on the column or not
            if (!typeof(IComparable).IsAssignableFrom(propertyType))
            {
                dataGridColumn.CanUserSort = false;
            }

            dataGridColumn.Header = itemProperty.Name;

            // Set the data field binding for such created columns and
            // choose the BindingMode based on editability of the property.
            DataGridBoundColumn boundColumn = dataGridColumn as DataGridBoundColumn;
            if (boundColumn != null || comboBoxColumn != null)
            {
                Binding binding = new Binding(itemProperty.Name);
                if (comboBoxColumn != null)
                {
                    comboBoxColumn.SelectedItemBinding = binding;
                }
                else
                {
                    boundColumn.Binding = binding;
                }

                PropertyDescriptor pd = itemProperty.Descriptor as PropertyDescriptor;
                if (pd != null)
                {
                    if (pd.IsReadOnly)
                    {
                        binding.Mode = BindingMode.OneWay;
                        dataGridColumn.IsReadOnly = true;
                    }
                }
                else
                {
                    PropertyInfo pi = itemProperty.Descriptor as PropertyInfo;
                    if (pi != null)
                    {
                        if (!pi.CanWrite)
                        {
                            binding.Mode = BindingMode.OneWay;
                            dataGridColumn.IsReadOnly = true;
                        }
                    }
                }
            }

            return  (BldTaskDataGridColumn)dataGridColumn ;
        }
        #endregion
        #region Data
        private DataGrid _dataGridOwner = null;                     // This property is updated by DataGrid when the column is added to the DataGrid.Columns collection

        #endregion

    }

}
