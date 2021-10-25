using System;
using System.Globalization;
using System.Windows.Data;

namespace IPD.Converter
{
    [ValueConversion(typeof(double), typeof(string))]
    public class StringDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("not double");
            }
            string v = ((double)value).ToString();
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(double))
            {
                throw new NotImplementedException();
            }
            if ((string)value == "") { return 0; }
            double v = double.Parse((string)value);
            return v;
        }
    }
}