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
        Task<List<Markierung>> ListMarkierungenByArtikel(int artikelId);
    }
}
