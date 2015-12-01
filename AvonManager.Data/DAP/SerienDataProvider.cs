using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.BusinessObjects;

namespace AvonManager.Data
{
    public class SerienDataProvider : AvonManager.Interfaces.ISerienDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public SerienDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<List<SerieDto>> ListAllSerien()
        {
            Task<List<SerieDto>> task = new Task<List<SerieDto>>(() =>
            {
                Common.Helpers.Logger.Current.Write("Retrieving data..");
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from c in database.Seriens
                                select c;

                    List<SerieDto> serienList = new List<SerieDto>();
                    foreach (Serien mark in query)
                    {
                        serienList.Add(_mapper.Map(mark));
                    }
                    Common.Helpers.Logger.Current.Write("...done.");
                    return serienList;
                };
            });
            task.Start();
            return task;
        }
        public Task<SerieDto> LoadSeriesById(int seriesId)
        {
            Task<SerieDto> task = new Task<SerieDto>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Seriens
                                select b;
                    Serien series = query.FirstOrDefault(x => x.SerienId == seriesId);
                    return _mapper.Map(series);
                }
            });
            task.Start();
            return task;
        }
        public void SaveSerie(SerieDto serie)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var ser = database.Seriens.Single(x => x.SerienId == serie.SerienId);
                ser.Name = serie.Name;
                ser.Bemerkung = serie.Bemerkung;
                ser.Logo = serie.Logo;
                ser.Parent = serie.ParentId;
                database.SubmitChanges();
            }
        }
        public void DeleteSerie(int serienId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Artikels
                            where b.SerienId == serienId
                            select b;
                foreach (var detail in query)
                {
                    detail.SerienId = null;
                }
                var kat = database.Seriens.Single(x => x.SerienId == serienId);
                database.Seriens.DeleteOnSubmit(kat);
                database.SubmitChanges();
            };
        }
        public int AddSerie(SerieDto serie)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Serien series = _mapper.Map(serie);
                database.Seriens.InsertOnSubmit(series);
                database.SubmitChanges();
                return series.SerienId;
            }
        }

        public Task<Dictionary<int, int>> ListArticleCountsBySeriesIds(int[] seriesIds)
        {
            Task<Dictionary<int, int>> task = new Task<Dictionary<int, int>>(() =>
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();
                if (seriesIds?.Length > 0)
                {
                    using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                    {
                        var query = from article in database.Artikels
                                    where article.SerienId.HasValue && seriesIds.Contains(article.SerienId.Value)
                                    group article by article.SerienId into grpCategorys
                                    select new
                                    {
                                        seriesId = grpCategorys.Key,
                                        cnt = grpCategorys.Count()
                                    };

                        foreach (var grp in query)
                        {
                            dic[grp.seriesId.Value] = grp.cnt;
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
