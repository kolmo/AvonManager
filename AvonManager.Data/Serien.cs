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
    
    public partial class Serien
    {
        public Serien()
        {
            this.Artikels = new HashSet<Artikel>();
            this.Serien1 = new HashSet<Serien>();
        }
    
        public int SerienId { get; set; }
        public string Name { get; set; }
        public Nullable<int> Parent { get; set; }
        public Nullable<int> Reihenfolge { get; set; }
        public string Bemerkung { get; set; }
        public byte[] Logo { get; set; }
    
        public virtual ICollection<Artikel> Artikels { get; set; }
        public virtual ICollection<Serien> Serien1 { get; set; }
        public virtual Serien Serien2 { get; set; }
    }
}
