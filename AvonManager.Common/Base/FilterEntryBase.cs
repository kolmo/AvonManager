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
                SetProperty(ref _isSelected, value);
            }
        }
        private bool _isSelectionEnabled;
        /// <summary>
        /// Gets or sets the IsSelectionEnabled.
        /// </summary>
        /// <value>
        /// The IsSelectionEnabled.
        /// </value>
        public bool IsSelectionEnabled
        {
            get { return _isSelectionEnabled; }
            set { SetProperty(ref _isSelectionEnabled, value); }
        }
    }
}
