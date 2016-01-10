using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IOrderDataProvider
    {
        Task<List<BestellungDto>> SearchOrders(IOrderSearchCriteria searchCriteria, int pageSize, int page);
        Task<BestellungDto> LoadOrder(int orderId);
        Task<List<BestelldetailDto>> ListOrderDetailsByOrder(int orderId);
        Task<List<BestellstatusDto>> ListAllOrderStatusValues();
        Task<List<int>> ListAllOrderYears();
        void UpdateOrder(BestellungDto order);
        void UpdateOrderDetail(BestelldetailDto orderDetail);
        int AddOrderDetail(BestelldetailDto orderDetail);
        void DeleteOrderDetail(int orderDetailId);
        void DeleteOrder(int orderId);
        int AddOrder(BestellungDto newOrder);
    }
}
