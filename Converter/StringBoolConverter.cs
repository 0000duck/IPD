using System;
using System.Globalization;
using System.Windows.Data;

namespace IPD.Converter
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class StringBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("not double");
            }
            int i = System.Convert.ToInt32((bool)value);
            string v = (i).ToString();
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new NotImplementedException();
            }
            bool v;
            if ((string)value == "0" || value == null)
            {
                v = false;
            }
            else
            {
                v = true;
            }
            return v;
        }
    }
}