using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public class TreeeViewCommandParToCommonCommandParConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list_value)
            {
                DataItem selected_dataItem = list_value[0] as DataItem;
                Type type_for_created_obj = null;
                if(list_value.Count>2)
                    type_for_created_obj= list_value[2] as Type;
                IList out_list = new List<object>
                            {   
                            selected_dataItem.AttachedObject ,
                            selected_dataItem.Parent.AttachedObject,
                            type_for_created_obj
                            };
                return out_list;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
