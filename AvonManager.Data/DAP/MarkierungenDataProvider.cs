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

        public Task<List<MarkierungDto>> ListAllMarkierungen()
        {
            Task<List<MarkierungDto>> task = new Task<List<MarkierungDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from c in database.Markierungens
                                select c;

                    List<MarkierungDto> markierungenList = new List<MarkierungDto>();
                    foreach (Markierungen mark in query)
                    {
                        markierungenList.Add(_mapper.Map<MarkierungDto>(mark));
                    }
                    return markierungenList;
                };
            });
            task.Start();
            return task;
        }

        public Task<List<MarkierungDto>> ListMarkierungenByArtikel(int artikelId)
        {
            Task<List<MarkierungDto>> task = new Task<List<MarkierungDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Markierungen_x_Artikels
                                join c in database.Markierungens 
                                on b.MarkierungId equals c.MarkierungId
                                where b.ArtikelId == artikelId
                                select c;

                    List<MarkierungDto> markierungenList = new List<MarkierungDto>();
                    foreach (Markierungen mark in query)
                    {
                        markierungenList.Add(_mapper.Map<BusinessObjects.MarkierungDto>(mark));
                    }
                    return markierungenList;
                };
            });
            task.Start();
            return task;
        }
    }
}
