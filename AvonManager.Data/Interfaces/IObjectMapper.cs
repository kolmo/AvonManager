using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Data.Interfaces
{
    public interface IObjectMapper
    {
        Target Map<Target>(object src)  where Target : class; 
       
    }
}
