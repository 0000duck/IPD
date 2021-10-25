using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using IPD.Model;
using Newtonsoft.Json;

namespace IPD.Converter
{
    [ValueConversion(typeof(SlaveTypeList), typeof(List<string>))]
    public class ServoSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> selectionList = new List<string>();
            SlaveTypeList sourceValue = (SlaveTypeList)value;
            selectionList.Add("虚拟伺服");
            if (JsonConvert.SerializeObject(sourceValue) == "{}")
            {
                return selectionList;
            }
            sourceValue.servoNum.ForEach(v =>
            {
                selectionList.Add(v.ToString() + sourceValue.slaveType[v - 1]);
            });
            return selectionList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}