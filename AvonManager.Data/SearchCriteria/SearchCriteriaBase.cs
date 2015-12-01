using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace AvonManager.Data
{
    public class SearchCriteriaBase : BindableBase, ICriteriaBase
    {
        public virtual void Reset() { }
    }
}
