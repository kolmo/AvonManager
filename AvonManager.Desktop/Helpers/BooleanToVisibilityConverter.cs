using System;
using System.Windows;
using System.Windows.Data;

namespace AvonManager.Helpers
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public BooleanToVisibilityConverter()
        {
            Inverted = false;
        }
        public bool Inverted { get; set; }
        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                bool isVisible = (bool)value;
                if (isVisible)
                {
                    return !Inverted ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    return !Inverted ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            else
            {
                return !Inverted ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
