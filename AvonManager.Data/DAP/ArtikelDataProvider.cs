using AvonManager.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvonManager.BusinessObjects;
using System;
using AvonManager.Interfaces.Criteria;

namespace AvonManager.Data
{
    public class ArtikelDataProvider : IArtikelDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public ArtikelDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }

        public int AddArticle(ArtikelDto newArticle)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Artikel artikel = _mapper.Map(newArticle);
                artikel.ChangedAt = DateTime.Now;
                database.Artikels.InsertOnSubmit(artikel);
                database.SubmitChanges();
                return artikel.ArtikelId;
            }
        }

        public void DeleteArticle(int articleId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Kategorien_x_Artikels
                            where b.ArtikelId == articleId
                            select b;
                foreach (var detail in query)
                {
                    database.Kategorien_x_Artikels.DeleteOnSubmit(detail);
                }
                var query2 = from b in database.Markierungen_x_Artikels
                            where b.ArtikelId == articleId
                            select b;
                foreach (var detail in query2)
                {
                    database.Markierungen_x_Artikels.DeleteOnSubmit(detail);
                }
                var article = database.Artikels.Single(x => x.ArtikelId== articleId);
                database.Artikels.DeleteOnSubmit(article);
                database.SubmitChanges();
            }
        }

        public Task<List<ArtikelDto>> ListAllArtikel()
        {
            Task<List<ArtikelDto>> task = new Task<List<BusinessObjects.ArtikelDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Artikels
                                orderby b.Artikelname
                                select b;

                    List<ArtikelDto> artikelList = new List<ArtikelDto>();
                    var page = query;
                    foreach (Artikel artikel in page)
                    {
                        artikelList.Add(_mapper.Map(artikel));
                    }
                    return artikelList;
                }
            });
            task.Start();
            return task;
        }

        public Task<ArtikelDto> LoadArtikel(int artikelId)
        {
            Task<ArtikelDto> task = new Task<BusinessObjects.ArtikelDto>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Artikels
                                orderby b.Artikelname
                                select b;
                    Artikel artikel = query.FirstOrDefault(x => x.ArtikelId == artikelId);
                    return _mapper.Map(artikel);
                }
            });
            task.Start();
            return task;
        }

        public void SaveArtikel(ArtikelDto dto)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Artikels
                            orderby b.Artikelname
                            select b;
                Artikel artikel = query.FirstOrDefault(x => x.ArtikelId == dto.ArtikelId);
                artikel.Artikelbeschreibung = dto.Beschreibung;
                artikel.Artikelname = dto.Name;
                artikel.ArtikelNr = dto.Nummer;
                artikel.Bild = dto.Bild;
                artikel.Einzelpreis = dto.Einzelpreis;
                artikel.Inhalt = dto.Inhalt;
                artikel.Lagerbestand = dto.Lagerbestand;
                artikel.SerienId = dto.SerienId;
                artikel.ChangedAt = DateTime.Now;
                try
                {
                    database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Task<List<ArtikelDto>> SearchArticles(IArticleSearchCriteria searchCriteria, int pageSize, int page)
        {
            Task<List<ArtikelDto>> task = new Task<List<ArtikelDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Artikels
                                select b;
                    if (searchCriteria != null)
                    {
                        if (searchCriteria.Categories?.Count() > 0)
                        {
                            query = from a in query
                                    join k in database.Kategorien_x_Artikels
                                    on a.ArtikelId equals k.ArtikelId
                                    where searchCriteria.Categories.Contains(k.KategorieId)
                                    select a;
                        }
                        if (searchCriteria.Markups?.Count() > 0)
                        {
                            //if (!invertedMarkups)
                            //{
                            query = from a in query
                                    join m in database.Markierungen_x_Artikels
                                    on a.ArtikelId equals m.ArtikelId
                                    where searchCriteria.Markups.Contains(m.MarkierungId)
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
                        if (searchCriteria.Series?.Count() > 0)
                        {
                            query = from a in query
                                    where a.SerienId.HasValue && searchCriteria.Series.Contains(a.SerienId.Value)
                                    select a;
                        }
                        if (!string.IsNullOrWhiteSpace(searchCriteria.FullText))
                        {
                            query = from a in query
                                    where (a.Artikelname != null && a.Artikelname.Contains(searchCriteria.FullText))
                                        || (a.Artikelbeschreibung != null && a.Artikelbeschreibung.Contains(searchCriteria.FullText))
                                    select a;
                        }
                    }
                    List<BusinessObjects.ArtikelDto> artikelList = new List<BusinessObjects.ArtikelDto>();
                    int recsToSkip = (page - 1) * pageSize;
                    var resultList = query.OrderByDescending(x=>x.ChangedAt).Skip(recsToSkip).Take(pageSize);
                    foreach (Artikel artikel in resultList)
                    {
                        artikelList.Add(_mapper.Map(artikel));
                    }
                    return artikelList;
                };
            });
            task.Start();
            return task;
        }
    }
}
