using AvonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.Data.Helpers;

namespace AvonManager.Data
{
    public class MarkierungenDataProvider : IMarkierungenDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public MarkierungenDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<List<BusinessObjects.Markierung>> ListMarkierungenByArtikel(int artikelId)
        {
            Task<List<BusinessObjects.Markierung>> task = new Task<List<BusinessObjects.Markierung>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Artikels
                                where b.ArtikelId == artikelId
                                select b;

                    List<BusinessObjects.Markierung> markierungenList = new List<BusinessObjects.Markierung>();
                    Artikel artikel = query.First();
                    foreach (Markierungen_x_Artikel mark in artikel.Markierungen_x_Artikels)
                    {
                        markierungenList.Add(_mapper.Map<BusinessObjects.Markierung>(mark.Markierungen));
                    }
                    return markierungenList;
                };
            });
            task.Start();
            return task;
        }
    }
}
