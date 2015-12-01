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
        Task<List<ArtikelDto>> ListAllArtikel();
        Task<List<ArtikelDto>> SearchArticles(Criteria.IArticleSearchCriteria searchCriteria, int pageSize, int page);
        int AddArticle(ArtikelDto newArticle);
        Task<ArtikelDto> LoadArtikel(int artikelId);
        void SaveArtikel(ArtikelDto artikel);
        void DeleteArticle(int articleId);
    }
}
