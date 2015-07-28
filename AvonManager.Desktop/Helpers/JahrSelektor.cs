using Microsoft.Practices.Prism.Mvvm;

namespace AvonManager.Helpers
{
    public class JahrSelektor : BindableBase
    {

        #region Fields
        private int _jahr;
        private bool _isSelected;
        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the Jahr.
        /// </summary>
        /// <value>
        /// The Jahr.
        /// </value>
        public int Jahr
        {
            get { return _jahr; }
            set
            {
                if (_jahr != value)
                {
                    _jahr = value;
                    OnPropertyChanged(() => this.Jahr);
                }
            }
        }

        /// <summary>
        /// Gets or sets the IsSelected.
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(() => this.IsSelected);
                }
            }
        }

        #endregion Properties

    }
}
