using System;
using System.Globalization;
using System.Windows.Data;

namespace IPD.Converter
{
    [ValueConversion(typeof(int), typeof(string))]
    public class StringIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("not double");
            }
            string v = ((int)value).ToString();
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(int))
            {
                throw new NotImplementedException();
            }
            if ((string)value == "") { return 0; }
            int v = int.Parse((string)value);
            return v;
        }
    }
}