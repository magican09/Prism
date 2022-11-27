using System;
using System.Windows;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public class BooleanToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType = null, object parameter = null, System.Globalization.CultureInfo culture = null)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
