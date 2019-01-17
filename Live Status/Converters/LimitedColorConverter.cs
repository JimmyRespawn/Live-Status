using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Live_Status.Helpers;

namespace Live_Status.Converters
{
    public class LimitedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString() == "\uE7BA")
                return "#FECE00";
            else
                return "#7EEC74";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
