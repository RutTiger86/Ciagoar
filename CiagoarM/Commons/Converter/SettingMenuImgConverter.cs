using CiagoarM.Languages;
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
    public class SettingMenuImgConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           String StettingTitle =  value.ToString();

            if (StettingTitle == Resource.Title_Setting_Item)
            {
                return "/Resources/Item_icon.png";
            }
            else if (StettingTitle == Resource.Title_Setting_Company)
            {
                return "/Resources/Company_icon.png";
            }
            else if (StettingTitle == Resource.Title_Setting_System)
            {
                return "/Resources/System_icon.png";
            }
            else
            {
                return "/Resources/User_icon.png";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
