using System;
using System.Globalization;
using System.Windows.Data;

namespace bldCustomControlLibrary
{
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DataGridExpandedItem)value)?.Object;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
