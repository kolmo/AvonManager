using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IKategorieProvider
    {
        Task<List<Kategorie>> ListAllKategorien();
        void SaveKategorie(Kategorie kategorie);
        int AddKategorie(Kategorie kategorie);
        void DeleteKategorie(Kategorie kategorie);
    }
}
