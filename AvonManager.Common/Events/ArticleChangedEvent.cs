using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Common.Events
{
    public class ArticleChangedEvent : PubSubEvent<ArtikelDto>
    {
    }
}
