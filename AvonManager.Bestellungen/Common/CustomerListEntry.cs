using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using Microsoft.Practices.Prism.Commands;
using System;

namespace AvonManager.Bestellungen.Common
{
    public class CustomerListEntry : FilterEntryBase
    {
        KundeDto _customer;
        public CustomerListEntry(KundeDto customer, Action<CustomerListEntry> selectAction, Action<CustomerListEntry> createNewOrderAction)
        {
            _customer = customer;
            SelectCommand = new DelegateCommand<CustomerListEntry>(selectAction);
            CreateOrderCommand = new DelegateCommand<CustomerListEntry>(createNewOrderAction);
        }
        public int ID { get { return _customer.KundenId; } }
        public string DisplayName { get { return $"{_customer.Nachname}, {_customer.Vorname}"; } }
        public KundeDto Customer { get { return _customer; } }
        public DelegateCommand<CustomerListEntry> SelectCommand { get; private set; }
        public DelegateCommand<CustomerListEntry> CreateOrderCommand { get; private set; }
    }
}
