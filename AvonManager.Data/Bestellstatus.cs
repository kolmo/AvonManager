//------------------------------------------------------------------------------
// <auto-generated>
//    Dieser Code wurde aus einer Vorlage generiert.
//
//    Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten Ihrer Anwendung.
//    Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AvonManager.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bestellstatus
    {
        public Bestellstatus()
        {
            this.Bestellungs = new HashSet<Bestellung>();
        }
    
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Beschreibung { get; set; }
    
        public virtual ICollection<Bestellung> Bestellungs { get; set; }
    }
}
