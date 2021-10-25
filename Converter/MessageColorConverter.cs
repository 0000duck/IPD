using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace IPD.Converter
{
    [ValueConversion(typeof(int), typeof(Brush))]
    public class MessageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                System.Diagnostics.Debug.WriteLine(targetType);
                throw new InvalidOperationException("不对");
            }
            SolidColorBrush brush = new SolidColorBrush();
            if ((int)value == 0)
            {
                brush.Color = Colors.White;
            }
            else if ((int)value == 1)
            {
                brush.Color = Colors.Yellow;
            }
            else if ((int)value == 2)
            {
                brush.Color = Colors.Red;
            }
            else
            {
                brush.Color = Colors.Red;
            }
            System.Diagnostics.Debug.WriteLine(brush.Color);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}