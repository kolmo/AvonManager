using AvonManager.Interfaces;
using Prism.Mvvm;

namespace AvonManager.Data
{
    public class SearchCriteriaBase : BindableBase, ICriteriaBase
    {
        public virtual void Reset() { }
    }
}
