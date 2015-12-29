using AvonManager.BusinessObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IKategorieProvider
    {
        Task<KategorieDto> LoadCategoryById(int categoryId);
        Task<List<KategorieDto>> ListAllKategorien();
        Task<List<ArticleCategoryDto>> ListKategorienByArtikel(int artikelId);
        Task<Dictionary<int, int>> ListArticleCountsByCategory(int[] categoryIds);
        void SaveKategorie(KategorieDto kategorie);
        int AddKategorie(KategorieDto kategorie);
        void DeleteKategorie(int kategorieId);
        void AddCategoryArtikel(ArticleCategoryDto articleCategory);
        void DeleteCategoryArtikel(ArticleCategoryDto articleCategory);
    }
}
