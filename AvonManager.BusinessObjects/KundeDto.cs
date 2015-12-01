using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class KundeDto
    {
        public int KundenId { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Adresse { get; set; }
        public string Ort { get; set; }
        public string Postleitzahl { get; set; }
        public string EmailAdresse { get; set; }
        public string TelefonPrivat { get; set; }
        public string TelefonBeruflich { get; set; }
        public string MobilesTelefon { get; set; }
        public string Faxnummer { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public string Anmerkungen { get; set; }
        public bool? BekommtHeft { get; set; }
        public byte[] Bild { get; set; }
        public bool? Inaktiv { get; set; }
    }
}
