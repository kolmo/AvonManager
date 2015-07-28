using System;
using System.Windows.Data;

namespace AvonManager.Helpers
{
    public class BooleanToTextConverter : IValueConverter
    {
        private const string JA = "Ja";
        private const string NEIN = "Nein";
        private const string KA = "k.A.";
        public BooleanToTextConverter()
        {
            Inverted = false;
        }
        public bool Inverted { get; set; }
        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                bool boolValue = (bool)value;
                if (boolValue)
                {
                    return !Inverted ? JA : NEIN;
                }
                else
                {
                    return !Inverted ? NEIN : JA;
                }
            }
            else
            {
                return KA;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
