using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces
{
    public interface IOrderSearchCriteria : ICriteriaBase
    {
        bool? DeletionReserved { get; set; }
        int? CustomerId { get; set; }
        string FullText { get; set; }
        string ArticleNumber { get; set; }
        string Campaign { get; set; }
        int[] OrderYears { get; set; }
    }
}
