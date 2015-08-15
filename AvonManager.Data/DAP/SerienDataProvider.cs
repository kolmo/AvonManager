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
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from c in database.Seriens
                                select c;

                    List<SerieDto> serienList = new List<SerieDto>();
                    foreach (Serien mark in query)
                    {
                        serienList.Add(_mapper.Map<SerieDto>(mark));
                    }
                    return serienList;
                };
            });
            task.Start();
            return task;
        }
    }
}
