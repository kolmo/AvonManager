using Microsoft.Practices.Prism.Mvvm;

namespace AvonManager.Common.Base
{
    public class FilterEntryBase : BindableBase
    {
        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
    }
}
