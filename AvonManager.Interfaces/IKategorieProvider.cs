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
        Task<List<KategorieDto>> ListAllKategorien();
        void SaveKategorie(KategorieDto kategorie);
        int AddKategorie(KategorieDto kategorie);
        void DeleteKategorie(KategorieDto kategorie);
    }
}
