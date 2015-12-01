using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class HeftDto
    {
        public int HeftId { get; set; }
        public string Titel { get; set; }
        public int? Jahr { get; set; }
        public string Beschreibung { get; set; }
        public byte[] Bild { get; set; }
    }
}
