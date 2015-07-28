

using Microsoft.Practices.Prism.Mvvm;
namespace AvonManager.Helpers
{
    public class InitialSelektor : BindableBase
    {

        private string _inital;
        /// <summary>
        /// Gets or sets the Initial.
        /// </summary>
        /// <value>
        /// The Initial.
        /// </value>
        public string Initial
        {
            get { return _inital; }
            set
            {
                if (_inital != value)
                {
                    _inital = value;
                    OnPropertyChanged(() => this.Initial);
                }
            }
        }

        private bool _isSelected;
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

    }
}
