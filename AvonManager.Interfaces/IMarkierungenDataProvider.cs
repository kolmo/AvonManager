using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IMarkierungenDataProvider
    {
        Task<MarkierungDto> LoadMarkingById(int markingId);
        Task<List<ArtikelMarkierungenDto>> ListMarkierungenByArtikel(int artikelId);
        Task<List<MarkierungDto>> ListAllMarkierungen(EntitaetDto entitaet);
        Task<Dictionary<int, int>> ListArticleCountsByMarkingIds(int[] markingIds);
        int AddMarking(MarkierungDto markierung);
        void DeleteMarking(int markierungId);
        void AddMarkierungArtikel(ArtikelMarkierungenDto articleMarking);
        void DeleteMarkierungArtikel(ArtikelMarkierungenDto articleMarking);
        void SaveMarkierung(MarkierungDto markierung);
    }
}
