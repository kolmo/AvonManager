using AvonManager.ArtikelModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AvonManager.ArtikelModule.Helpers
{
    public class CategoryCollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<ArticleCategoryViewModel> list = value as IEnumerable<ArticleCategoryViewModel>;
            if (list?.Count() > 0)
            {
                string separator = "; ";
                string conactedCats = list.Select(x => x.CategoryTitle).Aggregate(((seed, next) => seed + separator + next));
                return conactedCats.TrimEnd(separator.ToCharArray());
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
