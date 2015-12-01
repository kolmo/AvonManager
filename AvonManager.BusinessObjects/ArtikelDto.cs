using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class ArtikelDto
    {
        public int ArtikelId { get; set; }
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public string Nummer { get; set; }
        public string Inhalt { get; set; }
        public decimal? Einzelpreis { get; set; }
        public int? Lagerbestand { get; set; }
        public int? SerienId { get; set; }
        public byte[] Bild { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
