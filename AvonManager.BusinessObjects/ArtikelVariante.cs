using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class ArtikelVariante
    {
        public int VariantenId { get; set; }
        public int ArtikelId { get; set; }
        public string Artikelnummer { get; set; }
        public string Variante { get; set; }
        public int? Lagerbestand { get; set; }
        public decimal? Einzelpreis { get; set; }
    }
}
