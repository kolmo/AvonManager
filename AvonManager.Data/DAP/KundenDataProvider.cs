using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces;

namespace AvonManager.Data
{
    public class KundenDataProvider : IKundenDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public KundenDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }
        public int AddKunde(KundeDto customer)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Kunden customerEntity = _mapper.Map(customer);
                database.Kundens.InsertOnSubmit(customerEntity);
                database.SubmitChanges();
                return customerEntity.KundenId;
            }
        }

        public void DeleteKunde(int customerId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Hefte_x_Kundens
                            where b.KundenId == customerId
                            select b;
                foreach (var detail in query)
                {
                    database.Hefte_x_Kundens.DeleteOnSubmit(detail);
                }
                var customer = database.Kundens.Single(x => x.KundenId == customerId);
                database.Kundens.DeleteOnSubmit(customer);
                database.SubmitChanges();
            };
        }

        public Task<List<KundeDto>> SearchKunden(ICustomerSearchCriteria searchCriteria)
        {
            Task<List<KundeDto>> task = new Task<List<KundeDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Kundens
                                select b;

                    if (searchCriteria.BrochureId.HasValue)
                    {
                        query = from a in query
                                join b in database.Hefte_x_Kundens
                                on a.KundenId equals b.KundenId
                                where b.HeftId == searchCriteria.BrochureId
                                select a;
                    }
                    if (searchCriteria.GetsBrochure.HasValue)
                    {
                        query = from k in query
                                where k.BekommtHeft == searchCriteria.GetsBrochure.Value
                                select k;
                    }
                    if (searchCriteria.HasOrders.HasValue)
                    {
                        var innerquery = from o in database.Bestellungs select o.KundenId;
                        if (searchCriteria.HasOrders==true)
                        {
                            query = from k in query
                                    where innerquery.Contains(k.KundenId)
                                    select k;
                        }
                        else
                        {
                            query = from k in query
                                    where !innerquery.Contains(k.KundenId)
                                    select k;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(searchCriteria.SearchString))
                    {
                        query = from k in query
                                where k.Nachname != null && k.Nachname.Contains(searchCriteria.SearchString)
                                select k;
                    }
                    if (!string.IsNullOrWhiteSpace(searchCriteria.Initial))
                    {
                        query = from k in query
                                where k.Nachname != null && k.Nachname.ToUpper().StartsWith(searchCriteria.Initial.ToUpper())
                                select k;
                    }
                    if (searchCriteria.ActiveCustomersOnly == true)
                    {
                        query = from k in query
                                where k.Inaktiv == null || k.Inaktiv == false
                                select k;
                    }
                    if (searchCriteria.InActiveCustomersOnly == true)
                    {
                        query = from k in query
                                where k.Inaktiv == true 
                                select k;
                    }
                    query = from k in query
                            orderby k.Nachname
                            select k;

                    List<KundeDto> list = new List<KundeDto>();
                    foreach (Kunden kat in query.ToList())
                    {
                        list.Add(_mapper.Map(kat));
                    }
                    return list;
                };
            });
            task.Start();
            return task;
        }
        public void SaveKunde(KundeDto kunde)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Kundens.Single(x => x.KundenId == kunde.KundenId);
                if (entity != null)
                {
                    entity.Vorname = kunde.Vorname;
                    entity.Nachname = kunde.Nachname;
                    entity.Adresse = kunde.Adresse;
                    entity.Postleitzahl = kunde.Postleitzahl;
                    entity.Ort = kunde.Ort;
                    entity.TelefonBeruflich = kunde.TelefonBeruflich;
                    entity.TelefonPrivat = kunde.TelefonPrivat;
                    entity.MobilesTelefon = kunde.MobilesTelefon;
                    entity.Anmerkungen = kunde.Anmerkungen;
                    entity.EmailAdresse = kunde.EmailAdresse;
                    entity.Faxnummer = kunde.Faxnummer;
                    entity.BekommtHeft = kunde.BekommtHeft;
                    entity.Inaktiv = kunde.Inaktiv;
                    entity.Bild = kunde.Bild;
                    entity.Geburtsdatum = kunde.Geburtsdatum;
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

        public Task<List<KundeDto>> LoadCustomers(int[] customerIds)
        {
            Task<List<KundeDto>> task = new Task<List<KundeDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    List<KundeDto> list = new List<KundeDto>();
                    var query = database.Kundens.Where(x => customerIds.Contains( x.KundenId ));
                    foreach (Kunden kat in query.ToList())
                    {
                        list.Add(_mapper.Map(kat));
                    }
                    return list;
                }
            });
            task.Start();
            return task;
        }

        public Task<List<string>> GetAllCustomerInitials()
        {
            Task<List<string>> task = new Task<List<string>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    return database.Kundens.Where(x => x.Nachname != null && x.Nachname.Trim().Length > 0).Select(x => x.Nachname.Trim().ToUpper().Substring(0, 1)).Distinct().ToList();
                }
            });
            task.Start();
            return task;
        }
        public Task<Dictionary<int, int>> ListOrderCountsByCustomer(int[] customerIds)
        {
            Task<Dictionary<int, int>> task = new Task<Dictionary<int, int>>(() =>
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();
                if (customerIds?.Length > 0)
                {
                    using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                    {
                        var query = from o in database.Bestellungs
                                    where o.KundenId.HasValue && customerIds.Contains(o.KundenId.Value)
                                    group o by o.KundenId.Value into grpCustomers
                                    select new
                                    {
                                        customerId = grpCustomers.Key,
                                        cnt = grpCustomers.Count()
                                    };

                        foreach (var grp in query)
                        {
                            dic[grp.customerId] = grp.cnt;
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
