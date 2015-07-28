using AvonManager.BusinessObjects;
using AvonManager.Helpers.Messages;
using AvonManager.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.ServiceLocation;
using System.Collections.ObjectModel;
using System.Linq;

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
        private Bestellung _bestellungInProcess;
        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the bestellung information process.
        /// </summary>
        /// <value>
        /// The bestellung information process.
        /// </value>
        public Bestellung BestellungInProcess
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

        public IOrderedEnumerable<object> ClipboardList
        {
            get
            {
                if (_clipboardList == null)
                {
                    _clipboardList = ServiceLocator.Current.GetInstance<ClipboardManager>().Clipboard;
                    _clipboardList.CollectionChanged += (s, e) =>
                        {
                            OnPropertyChanged(() => this.ClipboardList);
                        };
                }
                return _clipboardList.OrderBy(x => x.ToString()); ;
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
        public DelegateCommand<object> ClearClipboard { get; private set; }
        public DelegateCommand<int> SelectTab { get; private set; }
        public DelegateCommand<object> SelectArtikelOrVariant { get; private set; }
        #endregion
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ClearClipboard = new DelegateCommand<object>(ClearClipboardAction);
            SelectTab = new DelegateCommand<int>(SelectTabAction);
            SelectArtikelOrVariant = new DelegateCommand<object>(SelectArtikelOrVariantAction);
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
        private void SelectArtikelOrVariantAction(object obj)
        {
            if (obj is Artikel)
            {
                //MessengerInstance.Send<PubSubEvent<Artikel>>(new PubSubEvent<Artikel>(obj as Artikel, null));
            }
            else if (obj is ArtikelVariante)
            {
                //MessengerInstance.Send<PubSubEvent<ArtikelVarianten>>(new PubSubEvent<ArtikelVarianten>(obj as ArtikelVarianten, null));
            }
        }
        #endregion Private helper methods

    }
}