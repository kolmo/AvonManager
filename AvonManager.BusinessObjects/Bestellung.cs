using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class Bestellung
    {
        public int BestellId { get; set; }
        public int KundenId { get; set; }
        public DateTime Datum { get; set; }
        public string Bemerkung { get; set; }
        public int StatusId { get; set; }
        public bool Loeschvormerkung { get; set; }
    }
}
