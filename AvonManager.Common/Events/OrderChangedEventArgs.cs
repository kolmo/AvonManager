using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Common.Events
{
    public class OrderChangedEventArgs : ChangedBaseEventArgs
    {
        public BusinessObjects.BestellungDto Order { get; set; }
    }
}
