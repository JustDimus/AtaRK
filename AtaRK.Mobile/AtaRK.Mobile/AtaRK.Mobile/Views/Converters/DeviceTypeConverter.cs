using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Views.Converters
{
    public class DeviceTypeConverter : IValueConverter
    {
        public const int DEVICETYPE_MAX_LENGTH = 10;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string deviceType)
            {
                if (deviceType.Length > DEVICETYPE_MAX_LENGTH)
                {
                    return string.Concat(deviceType.Substring(0, DEVICETYPE_MAX_LENGTH), "...");
                }

                return deviceType;
            }

            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
