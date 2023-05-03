using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public class ObjectsToListMultiConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<object> output_objects = new List<object>();
            foreach (object obj in values)
                output_objects.Add(obj);

            return output_objects;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
