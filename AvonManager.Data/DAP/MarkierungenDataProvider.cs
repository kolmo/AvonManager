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
    public class MarkierungenDataProvider : IMarkierungenDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public MarkierungenDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<List<MarkierungDto>> ListAllMarkierungen(EntitaetDto entitaet)
        {
            Task<List<MarkierungDto>> task = new Task<List<MarkierungDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from c in database.Markierungens
                                where c.EntitaetId == (int)entitaet
                                select c;

                    List<MarkierungDto> markierungenList = new List<MarkierungDto>();
                    foreach (Markierungen mark in query)
                    {
                        markierungenList.Add(_mapper.Map(mark));
                    }
                    return markierungenList;
                };
            });
            task.Start();
            return task;
        }

        public Task<List<ArtikelMarkierungenDto>> ListMarkierungenByArtikel(int artikelId)
        {
            Task<List<ArtikelMarkierungenDto>> task = new Task<List<ArtikelMarkierungenDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Markierungen_x_Artikels
                                where b.ArtikelId == artikelId
                                select b;

                    List<ArtikelMarkierungenDto> markierungenList = new List<ArtikelMarkierungenDto>();
                    foreach (Markierungen_x_Artikel mark in query)
                    {
                        markierungenList.Add(_mapper.Map(mark));
                    }
                    return markierungenList;
                };
            });
            task.Start();
            return task;
        }
        public Task<MarkierungDto> LoadMarkingById(int markingId)
        {
            Task<MarkierungDto> task = new Task<MarkierungDto>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Markierungens
                                select b;
                    Markierungen marking = query.FirstOrDefault(x => x.MarkierungId == markingId);
                    return _mapper.Map(marking);
                }
            });
            task.Start();
            return task;
        }
        public void SaveMarkierung(MarkierungDto dto)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Markierungens
                            orderby b.Titel
                            select b;
                Markierungen markierung = query.FirstOrDefault(x => x.MarkierungId == dto.MarkierungId);
                if (markierung != null)
                {
                    markierung.Titel = dto.Titel;
                    markierung.Bemerkung = dto.Bemerkung;
                    markierung.FarbeARGB = dto.FarbeARGB;
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
        }
        public int AddMarking(MarkierungDto markierung)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Markierungen marking = _mapper.Map(markierung);
                database.Markierungens.InsertOnSubmit(marking);
                database.SubmitChanges();
                return marking.MarkierungId;
            }
        }
        public void DeleteMarking(int markierungId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Markierungen_x_Artikels
                            where b.MarkierungId == markierungId
                            select b;
                foreach (var detail in query)
                {
                    database.Markierungen_x_Artikels.DeleteOnSubmit(detail);
                }
                var mark = database.Markierungens.Single(x => x.MarkierungId == markierungId);
                database.Markierungens.DeleteOnSubmit(mark);
                database.SubmitChanges();
            };
        }

        public void AddMarkierungArtikel(ArtikelMarkierungenDto articleMarking)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Markierungen_x_Artikels.SingleOrDefault(b => b.MarkierungId == articleMarking.MarkierungId && b.ArtikelId == articleMarking.ArtikelId);

                if (entity == null)
                {
                    database.Markierungen_x_Artikels.InsertOnSubmit(_mapper.Map(articleMarking));
                }
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
        public void DeleteMarkierungArtikel(ArtikelMarkierungenDto articleMarking)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Markierungen_x_Artikels.SingleOrDefault(b => b.MarkierungId == articleMarking.MarkierungId && b.ArtikelId == articleMarking.ArtikelId);
                if (entity != null)
                {
                    database.Markierungen_x_Artikels.DeleteOnSubmit(entity);
                    database.SubmitChanges();
                }
            }
        }

        public Task<Dictionary<int, int>> ListArticleCountsByMarkingIds(int[] markingIds)
        {
            Task<Dictionary<int, int>> task = new Task<Dictionary<int, int>>(() =>
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();
                if (markingIds?.Length > 0)
                {
                    using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                    {
                        var query = from c in database.Markierungen_x_Artikels
                                    where markingIds.Contains(c.MarkierungId)
                                    group c by c.MarkierungId into grpObject
                                    select new
                                    {
                                        id = grpObject.Key,
                                        cnt = grpObject.Count()
                                    };

                        foreach (var grp in query)
                        {
                            dic[grp.id] = grp.cnt;
                        }
                    }
                }
                return dic;
            });
            task.Start();
            return task;
        }
    }

}
