using AvonManager.BusinessObjects;
using AvonManager.Common.Base;

namespace AvonManager.Bestellungen.Common
{
    public class CustomerListEntry : FilterEntryBase
    {
        private KundeDto _customer;

        public CustomerListEntry(KundeDto customer)
        {
            _customer = customer;
        }

        public int ID
        { get { return _customer.KundenId; } }
        public string DisplayName
        { get { return $"{_customer.Nachname}, {_customer.Vorname}"; } }
        public bool? Inaktiv
        { get { return _customer.Inaktiv; } }
        public KundeDto Customer
        { get { return _customer; } }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}