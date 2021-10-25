using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace IPD.Converter
{
    [ValueConversion(typeof(int), typeof(List<string>))]
    public class RobotSwitchListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> switchList = new List<string>();
            switch ((int)value)
            {
                case 1:
                    switchList.Add("机器人1");
                    break;

                case 2:
                    switchList.Add("机器人1");
                    switchList.Add("机器人2");
                    break;

                case 3:
                    switchList.Add("机器人1");
                    switchList.Add("机器人2");
                    switchList.Add("机器人3");
                    break;

                case 4:
                    switchList.Add("机器人1");
                    switchList.Add("机器人2");
                    switchList.Add("机器人3");
                    switchList.Add("机器人4");
                    break;

                default:
                    break;
            }
            return switchList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}