using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CiagoarM.Commons.Converter
{
    public class SettingMenuConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color returncolor = Colors.LightGray;

            if (value is bool IsChecked)
            {
                if(IsChecked)
                {
                    returncolor  = (Color)ColorConverter.ConvertFromString("#E8F0FE");
                }
            }

            return new SolidColorBrush(returncolor);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
