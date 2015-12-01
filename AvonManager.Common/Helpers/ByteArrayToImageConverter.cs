using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AvonManager.Common.Helpers
{
    public class ByteArrayToImageConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is byte[] && (value as byte[]).Length > 0)
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = new MemoryStream(value as byte[]);
                bmi.EndInit();
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
