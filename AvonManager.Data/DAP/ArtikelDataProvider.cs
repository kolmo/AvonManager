using AvonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.Data.Helpers;
using AvonManager.BusinessObjects;

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

        public Task<List<BusinessObjects.Artikel>> SearchArticles(IEnumerable<int> categories, IEnumerable<int> series, IEnumerable<int> markups,
            bool invertedMarkups,
            string name,
            int pageSize, int page)
        {
            Task<List<BusinessObjects.Artikel>> task = new Task<List<BusinessObjects.Artikel>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Artikels
                                select b;
                    if (categories?.Count()>0)
                    {
                        query = from a in query
                                join k in database.Kategorien_x_Artikels
                                on a.ArtikelId equals k.ArtikelId
                                where categories.Contains(k.KategorieId)
                                select a;
                    }
                    if (markups?.Count()> 0)
                    {
                        //if (!invertedMarkups)
                        //{
                            query = from a in query
                                    join m in database.Markierungen_x_Artikels
                                    on a.ArtikelId equals m.ArtikelId
                                    where markups.Contains(m.MarkierungId)
                                    select a;
                        //}
                        //else
                        //{
                        //    query = from a in query
                        //            join m in database.Markierungen_x_Artikels
                        //            on a.ArtikelId equals m.ArtikelId
                        //            where !markups.Contains(m.MarkierungId)
                        //            select a;
                        //}
                    }
                    if (series?.Count() > 0)
                    {
                        query = from a in query
                                where a.SerienId.HasValue && series.Contains(a.SerienId.Value)
                                select a;
                    }
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        query = from a in query
                                where (a.Artikelname != null && a.Artikelname.Contains(name))
                                    || (a.Artikelbeschreibung != null && a.Artikelbeschreibung.Contains(name))
                                select a;
                    }

                    List<BusinessObjects.Artikel> artikelList = new List<BusinessObjects.Artikel>();
                    int recsToSkip = (page - 1) * pageSize;
                    var resultList = query.Skip(recsToSkip).Take(pageSize);
                    foreach (Artikel artikel in resultList)
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
