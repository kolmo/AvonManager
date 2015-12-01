using AvonManager.BusinessObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface ISerienDataProvider
    {
        Task<SerieDto> LoadSeriesById(int seriesId);
        Task<List<SerieDto>> ListAllSerien();
        Task<Dictionary<int, int>> ListArticleCountsBySeriesIds(int[] seriesIds);
        int AddSerie(SerieDto serie);
        void SaveSerie(SerieDto serie);
        void DeleteSerie(int serienId);
    }
}
