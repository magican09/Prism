using System;
using System.Collections.Generic;
using System.Text;

namespace bldCustomControlLibrary
{
    internal static class BldTaskDataGridHelper
    {
        public static bool ShouldNotifyRows(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Rows);
        }
        internal static bool ShouldNotifyRowSubtree(DataGridNotificationTarget target)
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
    }
}
