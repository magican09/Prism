using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public class TreeViewMultiCommandToCommandArgsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            CommandArgs commandArgs = new CommandArgs();
            if (values is IList list_value)
            {
                Type type_for_created_obj = null;
                DataItem selected_dataItem = list_value[0] as DataItem;
                if (list_value.Count > 1)
                    commandArgs.Buffet = list_value[1];
                if (list_value.Count > 2)
                    type_for_created_obj = list_value[2] as Type;

                if (selected_dataItem.Parent != null)
                    commandArgs.Parent = selected_dataItem.Parent.AttachedObject;

                commandArgs.Entity = selected_dataItem.AttachedObject;
                commandArgs.Type = type_for_created_obj;
                return commandArgs;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
