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
    
    public partial class ArtikelVarianten
    {
        public int VariantenId { get; set; }
        public int ArtikelId { get; set; }
        public string ArtikelNr { get; set; }
        public string Variante { get; set; }
        public Nullable<int> Lagerbestand { get; set; }
        public Nullable<decimal> Einzelpreis { get; set; }
    
        public virtual Artikel Artikel { get; set; }
    }
}
