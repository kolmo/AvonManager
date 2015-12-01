using AvonManager.BusinessObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IHefteDataProvider
    {
        Task<HeftDto> LoadHeft(int heftId);
        Task<List<HeftDto>> SearchBrochures(Criteria.IBrochureSearchCriteria criteria);
        Task<List<HeftKundeDto>> ListHeftKunden(int HeftId);
        Task<List<int>> ListAllYears();
        void SaveHeft(HeftDto heft);
        void SaveHeftKunde(HeftKundeDto heftKunde);
        void AddHeftKunde(HeftKundeDto heftKunde);
        void DeleteHeftKunde(HeftKundeDto heftKunde);
        int AddHeft(HeftDto heft);
        void DeleteHeft(int heftId);
    }
}
