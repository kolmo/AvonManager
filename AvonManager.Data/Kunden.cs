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
    
    public partial class Kunden
    {
        public Kunden()
        {
            this.Bestellungs = new HashSet<Bestellung>();
            this.Hefte_x_Kunden = new HashSet<Hefte_x_Kunden>();
        }
    
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
        public Nullable<System.DateTime> Geburtsdatum { get; set; }
        public string Anmerkungen { get; set; }
        public Nullable<bool> BekommtHeft { get; set; }
        public byte[] Bild { get; set; }
        public Nullable<bool> Inaktiv { get; set; }
    
        public virtual ICollection<Bestellung> Bestellungs { get; set; }
        public virtual ICollection<Hefte_x_Kunden> Hefte_x_Kunden { get; set; }
    }
}
