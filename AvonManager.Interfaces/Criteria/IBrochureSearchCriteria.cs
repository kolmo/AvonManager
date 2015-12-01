using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces.Criteria
{
    public interface IBrochureSearchCriteria : ICriteriaBase
    {
        string FullText { get; set; }
        int[] Years { get; set; }
    }
}
