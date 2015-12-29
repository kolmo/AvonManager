using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Interfaces.Criteria
{
    public interface IArticleSearchCriteria : ICriteriaBase
    {
        int[] Categories { get; set; }
        int[] Markups { get; set; }
        int[] Series { get; set; }
        bool WithoutMarkups { get; set; }
        string FullText { get; set; }
        bool WithoutSeries { get; set; }
        bool WithoutCategory { get; set; }
    }
}
