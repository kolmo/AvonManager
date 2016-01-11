using AvonManager.BusinessObjects;
using AvonManager.Helpers.Messages;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AvonManager.ViewModels
{
    public enum SelectedTabItem
    {
        Home = 0,
        Artikel,
        Bestellungen,
        Kunden,
        Serien
    }
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BindableBase
    {

        #region Fields
        private ObservableCollection<object> _clipboardList = null;
        private BestellungDto _bestellungInProcess;
        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the bestellung information process.
        /// </summary>
        /// <value>
        /// The bestellung information process.
        /// </value>
        public BestellungDto BestellungInProcess
        {
            get { return _bestellungInProcess; }
            set
            {
                if (_bestellungInProcess != value)
                {
                    _bestellungInProcess = value;
                    OnPropertyChanged(() => this.BestellungInProcess);
                }
            }
        }

        private int _selectedTab = (int)SelectedTabItem.Home;
        /// <summary>
        /// Gets or sets the SelectedTab.
        /// </summary>
        /// <value>
        /// The SelectedTab.
        /// </value>
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged(() => this.SelectedTab);
                }
            }
        }
        private object _selectedObject;
        /// <summary>
        /// Gets or sets the SelectedObject.
        /// </summary>
        /// <value>
        /// The SelectedObject.
        /// </value>
        public object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject != value)
                {
                    _selectedObject = value;
                    if (_selectedObject != null)
                    {
                        //MessengerInstance.Send<PubSubEvent<object>, BestellungViewModel>(new PubSubEvent<object>(_selectedObject, Constants.CLIPBOARD_SELECTED));
                    }
                }
            }
        }

        #region Commands
        public DelegateCommand<int> SelectTab { get; private set; }
        #endregion
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            SelectTab = new DelegateCommand<int>(SelectTabAction);
            //MessengerInstance.Register<BestellungEditMessage>(this, OnBestellungChanged);
        }

        #endregion Constructors

        #region Private helper methods
        private void OnBestellungChanged(BestellungEditMessage obj)
        {
            //BestellungInProcess = obj.IsEditing ? obj.Content : null;
        }
        private void ClearClipboardAction(object o)
        {
            this._clipboardList.Clear();
        }
        private void SelectTabAction(int tab)
        {
            SelectedTab = tab;
        }
      
        #endregion Private helper methods

    }
}