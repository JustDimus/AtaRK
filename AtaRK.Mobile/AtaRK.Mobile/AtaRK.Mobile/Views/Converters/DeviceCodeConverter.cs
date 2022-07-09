using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AtaRK.Mobile.Views.Converters
{
    public class DeviceCodeConverter : IValueConverter
    {
        private const int DEVICECODE_MAX_LENGTH = 25;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is string deviceCode)
            {
                if (deviceCode.Length > DEVICECODE_MAX_LENGTH)
                {
                    return string.Concat(deviceCode.Substring(0, DEVICECODE_MAX_LENGTH), "...");
                }

                return deviceCode;
            }

            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
