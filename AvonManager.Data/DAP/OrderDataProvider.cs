using AvonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.BusinessObjects;

namespace AvonManager.Data
{
    public class OrderDataProvider : IOrderDataProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public OrderDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }

        public int AddOrderDetail(BestelldetailDto orderDetail)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var order = database.Bestellungs.Single(x => x.BestellId == orderDetail.BestellId);
                order.ChangedAt = DateTime.Now;
                Bestelldetail detail = _mapper.Map(orderDetail);
                database.Bestelldetails.InsertOnSubmit(detail);
                database.SubmitChanges();
                return detail.DetailId;
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Bestelldetails
                            where b.BestellId == orderId
                            select b;
                foreach (var detail in query)
                {
                    database.Bestelldetails.DeleteOnSubmit(detail);
                }
                var order = database.Bestellungs.Single(x => x.BestellId == orderId);
                database.Bestellungs.DeleteOnSubmit(order);
                database.SubmitChanges();
            };
        }

        public void DeleteOrderDetail(int orderDetailId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {

                var detail = database.Bestelldetails.Single(x => x.DetailId == orderDetailId);
                var order = database.Bestellungs.Single(x => x.BestellId == detail.BestellId);
                order.ChangedAt = DateTime.Now;
                database.Bestelldetails.DeleteOnSubmit(detail);
                database.SubmitChanges();
            };
        }

        public Task<List<BestellstatusDto>> ListAllOrderStatusValues()
        {
            Task<List<BestellstatusDto>> task = new Task<List<BestellstatusDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var statusValues = database.Bestellstatus.ToList();
                    List<BestellstatusDto> list = new List<BestellstatusDto>();
                    foreach (var item in statusValues)
                    {
                        list.Add(_mapper.Map(item));
                    }
                    return list;
                }
            });
            task.Start();
            return task;
        }

        public Task<List<int>> ListAllOrderYears()
        {
            Task<List<int>> task = new Task<List<int>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var years = database.Bestelldetails.Where(x=>x.Jahr.HasValue).Select(x=>x.Jahr.Value).Distinct().ToList();
                    return years;
                }
            });
            task.Start();
            return task;
        }

        public Task<List<BestelldetailDto>> ListOrderDetailsByOrder(int orderId)
        {
            Task<List<BestelldetailDto>> task = new Task<List<BestelldetailDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Bestelldetails
                                where b.BestellId == orderId
                                select b;
                    List<BestelldetailDto> list = new List<BestelldetailDto>();
                    foreach (var item in query)
                    {
                        list.Add(_mapper.Map(item));
                    }
                    return list;
                }
            });
            task.Start();
            return task;
        }

        public Task<BestellungDto> LoadOrder(int orderId)
        {
            Task<BestellungDto> task = new Task<BestellungDto>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var entity = database.Bestellungs.Single(x => x.BestellId == orderId);
                    return _mapper.Map(entity);
                }
            });
            task.Start();
            return task;
        }

        public Task<List<BestellungDto>> SearchOrders(IOrderSearchCriteria searchCriteria, int pageSize, int page)
        {
            Task<List<BestellungDto>> task = new Task<List<BestellungDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Bestellungs
                                select b;
                    if (searchCriteria != null)
                    {
                        if (!string.IsNullOrWhiteSpace(searchCriteria.ArticleNumber))
                        {
                            var subQuery = from d in database.Bestelldetails
                                           where d.ArtikelNr != null && d.ArtikelNr.Contains(searchCriteria.ArticleNumber)
                                           group d by d.BestellId into grouping
                                           select grouping.Key;
                            query = from o in query
                                    where subQuery.Contains(o.BestellId)
                                    select o;
                        }
                        if (searchCriteria.CustomerIds?.Length > 0)
                        {
                            query = from o in query
                                    where o.KundenId.HasValue && searchCriteria.CustomerIds.Contains(o.KundenId.Value)
                                    select o;
                        }
                        if (searchCriteria.ActiveCustomersOnly)
                        {
                            var subQuery = from k in database.Kundens
                                           where k.Inaktiv == null || k.Inaktiv == false
                                           select k.KundenId;
                            query = from o in query
                                    where subQuery.Contains(o.KundenId.Value)
                                    select o;
                        }
                        if (!string.IsNullOrWhiteSpace( searchCriteria.Campaign))
                        {
                            var subQuery = from d in database.Bestelldetails
                                           where d.Campagne != null && d.Campagne.Contains(searchCriteria.Campaign)
                                           group d by d.BestellId into grouping
                                           select grouping.Key;
                            query = from o in query
                                    where subQuery.Contains(o.BestellId)
                                    select o;
                        }
                        if (!string.IsNullOrWhiteSpace(searchCriteria.FullText))
                        {
                            var subQuery = from d in database.Bestelldetails
                                           where (d.Artikelbeschr != null && d.Artikelbeschr.Contains(searchCriteria.FullText)) 
                                           ||(d.FGD != null && d.FGD.Contains(searchCriteria.FullText))
                                           group d by d.BestellId into grouping
                                           select grouping.Key;
                            query = from o in query
                                    where subQuery.Contains(o.BestellId)
                                    select o; 
                        }
                        if (searchCriteria.OrderYears?.Length>0)
                        {
                            query = from o in query
                                    join d in database.Bestelldetails
                                    on o.BestellId equals d.BestellId
                                    where d.Jahr.HasValue && searchCriteria.OrderYears.Contains(d.Jahr.Value)
                                    group o by o into grouping
                                    select grouping.Key;
                        }
                    }

                    query = from o in query
                            orderby o.Datum descending
                            select o;

                    List<BestellungDto> list = new List<BestellungDto>();
                    int recsToSkip = (page - 1) * pageSize;
                    var resultList = query.Skip(recsToSkip).Take(pageSize);
                    foreach (var item in resultList)
                    {
                        list.Add(_mapper.Map(item));
                    }
                    return list;
                }
            });
            task.Start();
            return task;
        }

        public int AddOrder(BestellungDto newOrder)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Bestellung order = _mapper.Map(newOrder);
                order.ChangedAt = DateTime.Now;
                database.Bestellungs.InsertOnSubmit(order);
                database.SubmitChanges();
                return order.BestellId;
            }

        }
        public void UpdateOrder(BestellungDto order)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Bestellungs.Single(x => x.BestellId == order.BestellId);
                entity.Bemerkung = order.Bemerkung;
                entity.Datum = order.Datum;
                entity.Loeschvormerkung = order.Loeschvormerkung;
                entity.StatusId = order.StatusId;
                entity.ChangedAt = DateTime.Now;
                database.SubmitChanges();
            };
        }

        public void UpdateOrderDetail(BestelldetailDto orderDetail)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var order = database.Bestellungs.Single(x => x.BestellId == orderDetail.BestellId);
                order.ChangedAt = DateTime.Now;

                var detail = database.Bestelldetails.Single(x => x.DetailId == orderDetail.DetailId);
                detail.Artikelbeschr = orderDetail.Artikelbeschreibung;
                detail.ArtikelNr = orderDetail.Artikelnummer;
                detail.Bemerkung = orderDetail.Bemerkung;
                detail.Campagne = orderDetail.Campagne;
                detail.Einzelpreis = orderDetail.Einzelpreis;
                detail.FGD = orderDetail.FDG;
                detail.Jahr = orderDetail.Jahr;
                detail.Menge = orderDetail.Menge;
                detail.Position = orderDetail.Position;
                detail.Seite = orderDetail.Seite;
                database.SubmitChanges();
            };
        }
    }
}
