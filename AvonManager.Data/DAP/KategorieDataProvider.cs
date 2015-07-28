using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.Data.Helpers;

namespace AvonManager.Data
{
    public class KategorieDataProvider : AvonManager.Interfaces.IKategorieProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public KategorieDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<List<Kategorie>> ListAllKategorien()
        {
            Task<List<BusinessObjects.Kategorie>> task = new Task<List<BusinessObjects.Kategorie>>(() =>
           {
               using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
               {
                   var query = from b in database.Kategoriens
                               orderby b.Name
                               select b;

                   List<BusinessObjects.Kategorie> list = new List<BusinessObjects.Kategorie>();
                   foreach (AvonManager.Data.Kategorien kat in query.ToList())
                   {
                       list.Add(_mapper.Map<BusinessObjects.Kategorie>(kat));
                   }
                   return list;
               };
           });
            task.Start();
            return task;
        }

        public void SaveKategorie(Kategorie kategorie)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var kat = database.Kategoriens.Single(x => x.KategorieId == kategorie.KategorieId);
                kat.Name = kategorie.Name;
                database.SubmitChanges();
            }
        }

        public void DeleteKategorie(Kategorie kategorie)
        {
            ;
        }


        public int AddKategorie(Kategorie kategorie)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                database.Kategoriens.InsertOnSubmit(_mapper.Map<Kategorien>(kategorie));
                database.SubmitChanges();
                return kategorie.KategorieId;
            }
        }
    }
}
