using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Input;

namespace AvonManager.KundenHefte.Common
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
                SetProperty(ref _inital, value);
            }
        }

        private ICommand _selectInitialCommand;
        /// <summary>
        /// Gets or sets the IsSelected.
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public ICommand SelectInitialCommand
        {
            get { return _selectInitialCommand; }
            set
            {
                SetProperty(ref _selectInitialCommand, value);
            }
        }

    }
}
