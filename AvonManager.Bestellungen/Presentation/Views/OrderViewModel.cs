using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;

namespace AvonManager.Bestellungen.Presentation.Views
{
    public class OrderViewModel : BindableBase
    {
        private BestellungDto _order;
        private KundeDto _customer;
        public OrderViewModel(BestellungDto order, KundeDto customer
            , Action<OrderViewModel> editOrderAction
            , Action<OrderViewModel> deleteOrderAction)
        {
            _order = order;
            _customer = customer;
            EditOrderCommand = new DelegateCommand<OrderViewModel>(editOrderAction);
            DeleteOrderCommand = new DelegateCommand<OrderViewModel>(deleteOrderAction);
        }
        public int OrderId { get { return _order.BestellId; } }
        public string CustomerName { get { return $"{_customer?.Nachname}, {_customer?.Vorname}"; } }
        public DateTime? OrderDate { get { return _order.Datum; } }
        public DelegateCommand<OrderViewModel> EditOrderCommand { get; private set; }
        public DelegateCommand<OrderViewModel> DeleteOrderCommand { get; private set; }
    }
}
