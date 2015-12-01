using AvonManager.BusinessObjects;
using AvonManager.Helpers;
using AvonManager.Helpers.Messages;
using AvonManager.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace AvonManager.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BestellungViewModel : BaseViewModel
    {


        #region Fields
        private Timer _executeFilterTimer;
        private bool _isAddingNewItem = false;
        private ClipboardManager _clipboardMgr;
        private BestelldetailDto _selectedDetail;
        private BestellungDto _selectedBestellung;
        private IEnumerable<KundeDto> _availableKunden;
        private IList<KundeDto> _sortedKunden4Filter;
        private string _suchString;
        private string _sucheArtikelNr;
        private ICommand _addNewBestellung;
        #endregion Fields

        #region Properties
        private IEnumerable<int> _sucheJahr;

        public IEnumerable<int> SucheJahr
        {
            get { return _sucheJahr; }
            set
            {
                if (_sucheJahr != value)
                {
                    _sucheJahr = value;
                }
            }
        }
        private string _sucheCampagne;

        public string SucheCampagne
        {
            get { return _sucheCampagne; }
            set
            {
                if (_sucheCampagne != value)
                {
                    _sucheCampagne = value;
                }
            }
        }

        public ICommand AddNewBestellung
        {
            get { return _addNewBestellung; }
            set
            {
                if (_addNewBestellung != value)
                {
                    _addNewBestellung = value;
                }
            }
        }


        private bool _isEditing;
        /// <summary>
        /// Gets or sets the IsEditing.
        /// </summary>
        /// <value>
        /// The IsEditing.
        /// </value>
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    BestellungIsReadOnly = !_isEditing;
                    OnPropertyChanged(() => this.IsEditing);
                }
            }
        }

        private bool _bestellungIsReadOnly = true;
        /// <summary>
        /// Gets or sets the BestellungIsReadOnly.
        /// </summary>
        /// <value>
        /// The BestellungIsReadOnly.
        /// </value>
        public bool BestellungIsReadOnly
        {
            get { return _bestellungIsReadOnly; }
            set
            {
                if (_bestellungIsReadOnly != value)
                {
                    _bestellungIsReadOnly = value;
                    OnPropertyChanged(() => this.BestellungIsReadOnly);
                }
            }
        }


        public string SucheArtikelNr
        {
            get { return _sucheArtikelNr; }
            set
            {
                if (_sucheArtikelNr != value)
                {
                    _sucheArtikelNr = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the SuchString.
        /// </summary>
        /// <value>
        /// The SuchString.
        /// </value>
        public string SuchString
        {
            get { return _suchString; }
            set
            {
                if (_suchString != value)
                {
                    _suchString = value;
                }
            }
        }
        private bool? _sucheLV;
        /// <summary>
        /// Gets or sets the SucheLV.
        /// </summary>
        /// <value>
        /// The SucheLV.
        /// </value>
        public bool? SucheLV
        {
            get { return _sucheLV; }
            set
            {
                if (_sucheLV != value)
                {
                    _sucheLV = value;
                }
            }
        }

        public IList<JahrSelektor> JahrListe { get; set; }
        public IList<KundeDto> SortedKunden4Filter { get { return _sortedKunden4Filter; } }

        private IList<KundeDto> _kunden4NeueBestellung;
        /// <summary>
        /// Gets or sets the Kunden4NeueBestellung.
        /// </summary>
        /// <value>
        /// The Kunden4NeueBestellung.
        /// </value>
        public IList<KundeDto> Kunden4NeueBestellung
        {
            get { return _kunden4NeueBestellung; }
            set
            {
                if (_kunden4NeueBestellung != value)
                {
                    _kunden4NeueBestellung = value;
                    OnPropertyChanged(() => this.Kunden4NeueBestellung);
                }
            }
        }

        private KundeDto _selectedKunde;
        /// <summary>
        /// Gets or sets the SelectedKunde.
        /// </summary>
        /// <value>
        /// The SelectedKunde.
        /// </value>
        public KundeDto SelectedKunde
        {
            get { return _selectedKunde; }
            set
            {
                if (_selectedKunde != value)
                {
                    _selectedKunde = value;
                }
            }
        }

        private string _selectedKundenName;
        /// <summary>
        /// Gets or sets the SelectedKundenName.
        /// </summary>
        /// <value>
        /// The SelectedKundenName.
        /// </value>
        public string SelectedKundenName
        {
            get { return _selectedKundenName; }
            set
            {
                if (_selectedKundenName != value)
                {
                    _selectedKundenName = value;
                    OnPropertyChanged(() => this.SelectedKundenName);
                }
            }
        }

        //public bool BestellungHasChanges { get { return _selectedBestellung != null ? _selectedBestellung.HasChanges : false; } }
        public string QueryName { get { return "GetBestellungFilteredByArtikel"; } }
        public int LoadSize { get { return 50; } }
        /// <summary>
        /// Gets or sets the SelectedDetails.
        /// </summary>
        /// <value>
        /// The SelectedDetails.
        /// </value>
        public BestelldetailDto SelectedDetail
        {
            get { return _selectedDetail; }
            set
            {
                if (_selectedDetail != value)
                {
                    _selectedDetail = value;
                    OnPropertyChanged(() => this.SelectedDetail);
                }
            }
        }
        /// <summary>
        /// Gets or sets the SelectedBestellung.
        /// </summary>
        /// <value>
        /// The SelectedBestellung.
        /// </value>
        public BestellungDto SelectedBestellung
        {
            get { return _selectedBestellung; }
            set
            {
                if (_selectedBestellung != value)
                {
                    _selectedBestellung = value;
                    if (_selectedBestellung != null)
                    {
                        if (_isAddingNewItem)
                        {
                            _isAddingNewItem = false;
                            SelectedBestellung.Datum = DateTime.Now;
                            //SelectedBestellung.Bestellstatus = Bestellstatuswerte != null ? Bestellstatuswerte.ElementAt(0) : null;
                            SelectedBestellung.Loeschvormerkung = false;
                            //MessengerInstance.Send<BestellungEditMessage>(new BestellungEditMessage(_selectedBestellung, "Edit") { IsEditing = true });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the AvailableKunden.
        /// </summary>
        /// <value>
        /// The AvailableKunden.
        /// </value>
        public IEnumerable<KundeDto> AvailableKunden
        {
            get { return _availableKunden; }
            set
            {
                if (_availableKunden != value)
                {
                    _availableKunden = value;
                    OnPropertyChanged(() => this.AvailableKunden);
                }
            }
        }
        public List<BestellstatusDto> Bestellstatuswerte { get { return null /*_ctx.Bestellstatus*/; } }
        public DelegateCommand<object> AddDetail { get; private set; }
        public DelegateCommand<object> RemoveDetail { get; private set; }
        public ICommand StartEditCommand { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the BestellungViewModel class.
        /// </summary>
        public BestellungViewModel()
        {
            _clipboardMgr = ServiceLocator.Current.GetInstance<ClipboardManager>();
            AddDetail = new DelegateCommand<object>(AddDetailAction, x=> {return true;});
            RemoveDetail = new DelegateCommand<object>(RemoveDetailAction, x => { return true; });
            //MessengerInstance.Register<PubSubEvent<Artikel>>(this, ReactOnArtikelFromClipboard);
            //MessengerInstance.Register<PubSubEvent<ArtikelVarianten>>(this, ReactOnArtikelVarianteFromClipboard);
            //MessengerInstance.Register<KundinAddedMessage>(this, OnKundinAdded);
            _executeFilterTimer = new Timer(RaiseFilterCallback);
            LoadSupplementData();
        }
        #endregion Constructors

        #region Override Methods
        protected override void FilterSetzenAction(object obj)
        {
            SetJahrFilterAction();
            RaiseFilterChanged();
        }
        /// <summary>
        /// Setzt die Filter auf ihren Ausgangswert zurück
        /// </summary>
        /// <param name="obj">The object.</param>
        protected override void FilterResetAction(object obj)
        {
            SuchString = string.Empty;
            SucheArtikelNr = string.Empty;
            SelectedKundenName = string.Empty;
            SelectedKunde = null;
            JahrListe.ToList().ForEach(x => x.IsSelected = false);
            SetJahrFilterAction();
            SucheCampagne = null;
            SucheLV = null;
            RaiseFilterChanged();
        }
        #endregion
        #region Public methods
        public void EditStarted()
        {
            IsEditing = true;
            //MessengerInstance.Send<BestellungEditMessage>(new BestellungEditMessage(SelectedBestellung, "Edit") { IsEditing = true });
        }
        public void BestellungAdded(object sender, EventArgs args)
        {
            _isAddingNewItem = true;
            IsEditing = true;
        }
        public void PageLoaded()
        {
            if (IsEditing)
            {
                return;
            }
        }

        private void LoadSupplementData()
        {
            //Context.Load(Context.GetBestellstatuswerteQuery());
            //IList<Heft> heftList = _clipboardMgr.GetAllOfType<Heft>();
            LoadAvailableKunden();
            LoadBestellungsjahre();
        }

        private void LoadBestellungsjahre()
        {
            //var result = Context.GetAllBestellungJahre(x =>
            //{
            //    if (!x.HasError)
            //    {
            //        IList<JahrSelektor> initialList = new List<JahrSelektor>();
            //        IList<int?> list = x.Value.OrderByDescending(xx => xx.Value).ToList();
            //        foreach (var jahr in list)
            //        {
            //            initialList.Add(new JahrSelektor { Jahr = jahr.Value });
            //        }
            //        JahrListe = initialList;
            //    }
            //}, null);
        }

        private void LoadAvailableKunden()
        {
            //Context.Load(Context.GetKundenQuery(), x =>
            //{
            //    if (!x.HasError)
            //    {
            //        AvailableKunden = x.Entities;
            //        _sortedKunden4Filter = AvailableKunden.OrderBy(xx => xx.DisplayName).ToList();
            //        Kunden4NeueBestellung = AvailableKunden.Where(xx => xx.Inaktiv == null || xx.Inaktiv == false).OrderBy(xx => xx.DisplayName).ToList();
            //    }
            //    OnPropertyChanged(() => this.SortedKunden4Filter);
            //}, null);
        }
        #endregion Public methods

        #region Private helper methods
        private void AddDetailAction(object obj)
        {
            //if (SelectedBestellung != null && SelectedBestellung.Bestelldetails != null)
            //{
            //    Bestelldetail neuesDetail = new Bestelldetail();
            //    PrefillDetails(neuesDetail);
            //    //SelectedBestellung.Bestelldetails.Add(neuesDetail);
            //    SelectedDetail = neuesDetail;
            //    if (StartEditCommand != null)
            //    {
            //        StartEditCommand.Execute(null);
            //    }
            //}
        }
        private void RemoveDetailAction(object obj)
        {
            //if (SelectedBestellung != null && SelectedBestellung.Bestelldetails != null && SelectedDetail != null)
            //{
            //    Bestelldetail av = SelectedDetail;
            //    SelectedDetail = null;
            //    //SelectedBestellung.Bestelldetails.Remove(av);
            //}
        }
     
        private void PrefillDetails(BestelldetailDto detail)
        {
            //if (SelectedBestellung.Bestelldetails.Any())
            //{
            //    Bestelldetail last = SelectedBestellung.Bestelldetails.Last();
            //    detail.Jahr = last.Jahr;
            //    detail.Campagne = last.Campagne;
            //    detail.Seite = last.Seite;
            //}
            //else
            //{
            //    detail.Jahr = DateTime.Now.Year;
            //    detail.Menge = 1;
            //}
        }
        void _selectedBestellung_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //OnPropertyChanged(() => this.BestellungHasChanges);
        }
        private void ReactOnArtikelFromClipboard(PubSubEvent<ArtikelDto> msg)
        {
            //CopyArtikelToBestelldetail(msg.Content);
        }

        private void OnKundinAdded(KundinAddedMessage msg)
        {
            //Context.Kundens.Clear();
            LoadAvailableKunden();
        }
        private void RaiseFilterCallback(object obj)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(RaiseFilterChanged);
        }
        private void RaiseFilterChanged()
        {
            OnPropertyChanged(() => SelectedKunde);
            OnPropertyChanged(() => SuchString);
            OnPropertyChanged(() => SucheArtikelNr);
            OnPropertyChanged(() => SucheJahr);
            OnPropertyChanged(() => SucheCampagne);
            OnPropertyChanged(() => SucheLV);
        }
        private void SetJahrFilterAction()
        {
            SucheJahr = JahrListe.Where(x => x.IsSelected).Select(x => x.Jahr).ToList();
        }
        #endregion Private helper methods

    }
}