﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvonManager.BusinessObjects
{
    public class KategorieDto
    {
        public int KategorieId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? Reihenfolge { get; set; }
        public string Bemerkung { get; set; }
        public byte[] Logo { get; set; }
    }
}
