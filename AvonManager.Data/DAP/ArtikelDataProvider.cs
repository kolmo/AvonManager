using AvonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.Data.Helpers;

namespace AvonManager.Data
{
    public class ArtikelDataProvider : IArtikelDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public ArtikelDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<List<BusinessObjects.Artikel>> ListAllArtikel()
        {
            Task<List<BusinessObjects.Artikel>> task = new Task<List<BusinessObjects.Artikel>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Artikels
                                orderby b.Artikelname
                                select b;

                    List<BusinessObjects.Artikel> artikelList = new List<BusinessObjects.Artikel>();
                    var page = query;
                    foreach (Artikel artikel in page)
                    {
                        artikelList.Add(_mapper.Map<BusinessObjects.Artikel>(artikel));
                    }
                    return artikelList;
                };
            });
            task.Start();
            return task;
        }
    }
}
