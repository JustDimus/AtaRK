using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Views.Converters
{
    public class DeviceTitleConverter : IValueConverter
    {
        public const int DEVICETITLE_MAX_LENGTH = 26;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string deviceTitle)
            {
                if (deviceTitle.Length > DEVICETITLE_MAX_LENGTH)
                {
                    return string.Concat(deviceTitle.Substring(0, DEVICETITLE_MAX_LENGTH), "...");
                }

                return deviceTitle;
            }

            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
