using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class BestellstatusDto
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Beschreibung { get; set; }
    }
}
