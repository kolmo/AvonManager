using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace AvonManager.Helpers
{
    public class CollectionToCountConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable)
            {
                return new List<object>((value as IEnumerable).Cast<object>()).Count;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
