using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace bldCustomControlLibrary
{
    internal static class DataGridHelper
    {
        #region Notification Propagation

        public static bool ShouldNotifyCells(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Cells);
        }

        public static bool ShouldNotifyCellsPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.CellsPresenter);
        }

        public static bool ShouldNotifyColumns(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Columns);
        }

        public static bool ShouldNotifyColumnHeaders(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnHeaders);
        }

        public static bool ShouldNotifyColumnHeadersPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnHeadersPresenter);
        }

        public static bool ShouldNotifyColumnCollection(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnCollection);
        }

        public static bool ShouldNotifyDataGrid(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.DataGrid);
        }

        public static bool ShouldNotifyDetailsPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.DetailsPresenter);
        }

        public static bool ShouldRefreshCellContent(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.RefreshCellContent);
        }

        public static bool ShouldNotifyRowHeaders(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.RowHeaders);
        }

        public static bool ShouldNotifyRows(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Rows);
        }

        public static bool ShouldNotifyRowSubtree(DataGridNotificationTarget target)
        {
            DataGridNotificationTarget value =
                DataGridNotificationTarget.Rows |
                DataGridNotificationTarget.RowHeaders |
                DataGridNotificationTarget.CellsPresenter |
                DataGridNotificationTarget.Cells |
                DataGridNotificationTarget.RefreshCellContent |
                DataGridNotificationTarget.DetailsPresenter;

            return TestTarget(target, value);
        }

        private static bool TestTarget(DataGridNotificationTarget target, DataGridNotificationTarget value)
        {
            return (target & value) != 0;
        }

        #endregion

        #region Tree Helpers

        /// <summary>
        ///     Walks up the templated parent tree looking for a parent type.
        /// </summary>
        public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }

            return null;
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        /// <summary>
        ///     Helper method which determines if any of the elements of
        ///     the tree is focusable and has tab stop
        /// </summary>
        public static bool TreeHasFocusAndTabStop(DependencyObject element)
        {
            if (element == null)
            {
                return false;
            }

            UIElement uielement = element as UIElement;
            if (uielement != null)
            {
                if (uielement.Focusable && KeyboardNavigation.GetIsTabStop(uielement))
                {
                    return true;
                }
            }
            else
            {
                ContentElement contentElement = element as ContentElement;
                if (contentElement != null && contentElement.Focusable && KeyboardNavigation.GetIsTabStop(contentElement))
                {
                    return true;
                }
            }

            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i) as DependencyObject;
                if (TreeHasFocusAndTabStop(child))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
