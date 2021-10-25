using System;
using System.Globalization;
using System.Windows.Data;

namespace IPD.Converter
{
    [ValueConversion(typeof(bool), typeof(object))]
    public class ConnectStateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(object))
            {
                throw new InvalidOperationException("not bool");
            }
            return (bool)value ? "连接成功" : "连接";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}