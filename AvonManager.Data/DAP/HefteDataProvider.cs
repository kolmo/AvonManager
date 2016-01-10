using AvonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces.Criteria;

namespace AvonManager.Data
{
    public class HefteDataProvider : IHefteDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public HefteDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }

        public int AddHeft(HeftDto heft)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Hefte heftEntity = _mapper.Map(heft);
                database.Heftes.InsertOnSubmit(heftEntity);
                database.SubmitChanges();
                return heftEntity.HeftId;
            }
        }

        public void AddHeftKunde(HeftKundeDto heftKunde)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Hefte_x_Kundens.SingleOrDefault(x => x.HeftId == heftKunde.HeftId && x.KundenId == heftKunde.KundenId);
                if (entity == null)
                {
                    database.Hefte_x_Kundens.InsertOnSubmit(_mapper.Map(heftKunde));
                    database.SubmitChanges();
                }
            }
        }

        public void DeleteHeft(int heftId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Hefte_x_Kundens
                            where b.HeftId == heftId
                            select b;
                foreach (var detail in query)
                {
                    database.Hefte_x_Kundens.DeleteOnSubmit(detail);
                }
                var brochure = database.Heftes.Single(x => x.HeftId == heftId);
                database.Heftes.DeleteOnSubmit(brochure);
                database.SubmitChanges();
            };
        }

        public void DeleteHeftKunde(HeftKundeDto heftKunde)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Hefte_x_Kundens.SingleOrDefault(x => x.HeftId == heftKunde.HeftId && x.KundenId == heftKunde.KundenId);
                if (entity != null)
                {
                    database.Hefte_x_Kundens.DeleteOnSubmit(entity);
                    database.SubmitChanges();
                }
            }
        }

        public Task<List<HeftDto>> SearchBrochures(IBrochureSearchCriteria criteria)
        {
            Task<List<HeftDto>> task = new Task<List<HeftDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Heftes
                                select b;

                    if (criteria!= null)
                    {
                        if (criteria.Years?.Length > 0)
                        {
                            query = from b in query
                                    where b.Jahr.HasValue && criteria.Years.Contains(b.Jahr.Value)
                                    select b;
                        }
                        if (!string.IsNullOrWhiteSpace(criteria.FullText))
                        {
                            query = from b in query
                                    where (b.Titel != null && b.Titel.Contains(criteria.FullText)) || (b.Beschreibung != null && b.Beschreibung.Contains(criteria.FullText))
                                    select b;
                        }
                    }
                    query = from b in query
                            orderby b.Jahr descending
                            select b;

                    List<HeftDto> list = new List<HeftDto>();
                    foreach (Hefte kat in query.ToList())
                    {
                        list.Add(_mapper.Map(kat));
                    }
                    return list;
                };
            });
            task.Start();
            return task;
        }

        public Task<List<int>> ListAllYears()
        {
            Task<List<int>> task = new Task<List<int>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Heftes
                                where b.Jahr.HasValue
                                group b by b.Jahr;
                    List<int> result = new List<int>();
                    foreach (var group in query.OrderByDescending(x => x.Key))
                    {
                        result.Add(group.Key.Value);
                    }
                    return result;
                };
            });
            task.Start();
            return task;
        }

        public Task<List<HeftKundeDto>> ListHeftKunden(int heftId)
        {
            Task<List<HeftKundeDto>> task = new Task<List<HeftKundeDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Hefte_x_Kundens
                                where b.HeftId == heftId
                                select b;
                    List<HeftKundeDto> result = new List<HeftKundeDto>();
                    foreach (var hk in query)
                    {
                        result.Add(_mapper.Map(hk));
                    }
                    return result;
                };
            });
            task.Start();
            return task;
        }

        public Task<HeftDto> LoadHeft(int heftId)
        {
            Task<HeftDto> task = new Task<HeftDto>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    Hefte heft = database.Heftes.Single(x => x.HeftId == heftId);
                    return _mapper.Map(heft);
                }
            });
            task.Start();
            return task;
        }

        public void SaveHeft(HeftDto heft)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Heftes.Single(x => x.HeftId == heft.HeftId);
                entity.Titel = heft.Titel;
                entity.Beschreibung = heft.Beschreibung;
                entity.Bild = heft.Bild;
                entity.Jahr = heft.Jahr;
                database.SubmitChanges();
            }
        }

        public void SaveHeftKunde(HeftKundeDto heftKunde)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Hefte_x_Kundens.SingleOrDefault(x => x.HeftId == heftKunde.HeftId && x.KundenId == heftKunde.KundenId);
                if (entity != null)
                {
                    entity.HatBestellt = heftKunde.HasOrdered;
                    entity.Erhalten = heftKunde.ReceivedAt;
                    database.SubmitChanges();
                }
            }
        }
    }
}
