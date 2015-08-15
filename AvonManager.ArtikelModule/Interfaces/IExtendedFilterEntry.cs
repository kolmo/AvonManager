using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.ArtikelModule.Interfaces
{
    public interface IExtendedFilterEntry : IFilterEntry
    {
        byte[] Symbol { get; }
        int? ColorCode { get; }
    }
}
