using System;
using System.Globalization;
using Xamarin.Forms;

namespace MarvelFinder.Converters
{
    public class StringFormattedDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return DateTime.Parse((string)value).ToString("dd/MM/yyyy");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

