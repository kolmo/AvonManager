using AvonManager.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;

namespace AvonManager.Helpers
{
    public class Serie4TreeConverter : IValueConverter
    {

        #region IValueConverter Member

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Serien> coll = value as List<Serien>;
            if (coll!=null && coll is INotifyCollectionChanged)
            {
                return new ObservableCollection<Serien>(coll);
            }
            else
            {
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
