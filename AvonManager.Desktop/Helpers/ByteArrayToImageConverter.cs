using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AvonManager.Helpers
{
    public class ByteArrayToImageConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is byte[])
            {
                BitmapImage bmi = new BitmapImage();
                bmi.StreamSource = new MemoryStream(value as byte[]);
                return bmi;
            }
            else if (value is WriteableBitmap)
            {
                return value;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
