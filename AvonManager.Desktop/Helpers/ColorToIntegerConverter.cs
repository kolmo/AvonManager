using System;
using System.Windows.Data;
using System.Windows.Media;

namespace AvonManager.Helpers
{
    /// <summary>
    /// Konvertiert eine Farbe zu einem Integerwert, und umgekehrt.
    /// </summary>
    public class ColorToIntegerConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                int iCol = (int)value;
                return Color.FromArgb(
                    (byte)(iCol >> 24)
                    , (byte)(iCol >> 16)
                    , (byte)(iCol >> 8)
                    , (byte)(iCol));
            }
            else
                return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Color)
            {
                Color c = (Color)value;
                var result = (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;
                return result;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }

    public class IntegerToBrushConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                int iCol = (int)value;
                return new SolidColorBrush(Color.FromArgb(
                    (byte)(iCol >> 24)
                    , (byte)(iCol >> 16)
                    , (byte)(iCol >> 8)
                    , (byte)(iCol)));
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
