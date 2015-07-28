using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class Markierung
    {
        public int MarkierungId { get; set; }
        public int EntitaetId { get; set; }
        public string Titel { get; set; }
        public string Bemerkung { get; set; }
        public int? FarbeARGB { get; set; }
    }
}
