using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace bldCustomControlLibrary 
{
    public abstract class BldTaskDataGridColumn : DataGridColumn
    {

        #region Oner Communication
        protected internal DataGrid DataGridOwner
        {
            get { return _dataGridOwner; }
            internal set { _dataGridOwner = value; }
        }
        #region

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
        #region Data
        private DataGrid _dataGridOwner = null;                     // This property is updated by DataGrid when the column is added to the DataGrid.Columns collection

        #endregion
    }
}
