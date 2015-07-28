using System;
using System.Windows.Data;

namespace AvonManager.Helpers
{
    public class IsCheckedToTextConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            bool? bv = value as bool?;

            string sw = parameter as string;
            switch (sw)
            {
                case "Kunden":
                    if (!bv.HasValue)
                    {
                        return "Alle Kunden";
                    }
                    else if (bv.Value == true)
                    {
                        return "Nur aktive Kunden";
                    }
                    else
                    {
                        return "Nur inaktive Kunden";
                    }
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
