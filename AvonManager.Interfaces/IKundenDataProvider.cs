using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IKundenDataProvider
    {
        Task<List<KundeDto>> LoadCustomers(int[] customerIds);
        Task<List<KundeDto>> SearchKunden(ICustomerSearchCriteria searchCriteria);
        Task<List<string>> GetAllCustomerInitials();
        void SaveKunde(KundeDto heft);
        int AddKunde(KundeDto heft);
        void DeleteKunde(int customerId);
    }
}
