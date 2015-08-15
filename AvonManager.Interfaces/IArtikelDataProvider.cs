using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IArtikelDataProvider
    {
        Task<List<Artikel>> ListAllArtikel();
        /// <summary>
        /// Sucht Artikel
        /// </summary>
        /// <param name="categories">Liste der Kategorie-IDs</param>
        /// <param name="series">Liste der Serien-IDS</param>
        /// <param name="markups">Liste der Markierungen-IDs</param>
        /// <param name="name">Suchstring</param>
        /// <param name="pageSize">Seitengroesse</param>
        /// <param name="page">Seitennummer</param>
        /// <returns></returns>
        Task<List<Artikel>> SearchArticles(IEnumerable<int> categories, IEnumerable<int> series, IEnumerable<int> markups, bool invertedMarkups, string name, int pageSize, int page);
    }
}
