using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class BestelldetailDto
    {
        public int DetailId { get; set; }
        public int BestellId { get; set; }
        public string Campagne { get; set; }
        public int? Jahr { get; set; }
        public int? Seite { get; set; }
        public string Artikelnummer { get; set; }
        public int? Menge { get; set; }
        public string Artikelbeschreibung { get; set; }
        public string FDG { get; set; }
        public decimal? Einzelpreis { get; set; }
    }
}
