using AvonManager.ArtikelModule.Interfaces;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ArtikelSearchPageViewModel : BaseViewModel
    {
        int _pageSize = 5;
        int _page = 0;
        IArtikelDataProvider _dataProvider;
        IMarkierungenDataProvider _markierungenDataProvider;
        IKategorieProvider _kategorienDataProvider;
        ISerienDataProvider _seriendataProvider;
        public ArtikelSearchPageViewModel(IArtikelDataProvider dataProvider,
            IMarkierungenDataProvider markierungenDataProvider,
            IKategorieProvider kategorienDataProvider,
            ISerienDataProvider seriendataProvider) : this()
        {
            _dataProvider = dataProvider;
            _markierungenDataProvider = markierungenDataProvider;
            _kategorienDataProvider = kategorienDataProvider;
            _seriendataProvider = seriendataProvider;
            KategorienFilter = new ObservableCollection<IFilterEntry>();
            MarkierungenFilter = new ObservableCollection<IFilterEntry>();
            SerienFilter = new ObservableCollection<IFilterEntry>();
        }
        #region Props

        private ObservableCollection<ArtikelViewModel> _artikelListe = new ObservableCollection<ArtikelViewModel>();
        /// <summary>
        /// Gets or sets the ArtikelListe.
        /// </summary>
        /// <value>
        /// The ArtikelListe.
        /// </value>
        public ObservableCollection<ArtikelViewModel> ArtikelListe
        {
            get { return _artikelListe; }
            set
            {
                if (_artikelListe != value)
                {
                    _artikelListe = value;
                    OnPropertyChanged(() => this.ArtikelListe);
                }
            }
        }

        private string _sucheArtikelNr;
        /// <summary>
        /// Gets or sets the SucheArtikelNr.
        /// </summary>
        /// <value>
        /// The SucheArtikelNr.
        /// </value>
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
        #endregion
        public async void LoadData()
        {
            var kategorien = await _kategorienDataProvider.ListAllKategorien();
            foreach (KategorieDto item in kategorien)
            {
                IFilterEntry vm = new Common.KategorieFilterEntry(item);
                KategorienFilter.Add(vm);
            }
            var markierungen = await _markierungenDataProvider.ListAllMarkierungen();
            MarkierungenFilter.Clear();
            MarkierungenFilter.Add(new Common.AlleMarkierungenFilterEntry());
            foreach (var item in markierungen)
            {
                IFilterEntry vm = new Common.MarkierungFilterEntry(item);
                MarkierungenFilter.Add(vm);
            }
            SelectedMarkierungFilterEntry = MarkierungenFilter[0];
            var serien = await _seriendataProvider.ListAllSerien();
            SerienFilter.Clear();
            SerienFilter.Add(new Common.AlleSerienFilterEntry());
            foreach (var item in serien.OrderBy(x => x.Name))
            {
                IFilterEntry vm = new Common.SerieFilterEntry(item);
                SerienFilter.Add(vm);
            }
            SelectedSerienFilterEntry = SerienFilter[0];
        }
        private List<DelegateCommand<object>> commands = new List<DelegateCommand<object>>();

        #region Fields
        private const int STD_WAIT = 2000;
        //private ClipboardManager _clipboardMgr;
        //private AvDBaseEntities _ctx;
        private List<int> _katIds;
        private List<int> _serienIds;
        private List<int> _markierungenIds;
        private string _suchString;
        private ArtikelVariante _selectedVariante;
        private SerieDto _selectedSerienDec;
        private ICommand _reloadData;
        private System.Threading.Timer _executeFilterTimer;
        #endregion

        #region Properties

        private bool _bestellungIsEditing = false;
        /// <summary>
        /// Gets or sets the BestellungIsEditing.
        /// </summary>
        /// <value>
        /// The BestellungIsEditing.
        /// </value>
        public bool BestellungIsEditing
        {
            get { return _bestellungIsEditing; }
            set
            {
                if (_bestellungIsEditing != value)
                {
                    _bestellungIsEditing = value;
                    OnPropertyChanged(() => this.BestellungIsEditing);
                }
            }
        }
        private object _checkedItem;
        /// <summary>
        /// Gets or sets the CheckedItem.
        /// </summary>
        /// <value>
        /// The CheckedItem.
        /// </value>
        public object CheckedKategorie
        {
            get { return _checkedItem; }
            set
            {
                if (_checkedItem != value)
                {
                    _checkedItem = value;
                    OnPropertyChanged(() => this.CheckedKategorie);
                }
            }
        }
        private object _checkedMarkierung;
        /// <summary>
        /// Gets or sets the CheckedMarkierung.
        /// </summary>
        /// <value>
        /// The CheckedMarkierung.
        /// </value>
        public object CheckedMarkierung
        {
            get { return _checkedMarkierung; }
            set
            {
                if (_checkedMarkierung != value)
                {
                    _checkedMarkierung = value;
                    OnPropertyChanged(() => this.CheckedMarkierung);
                }
            }
        }
        private object _checkedSerie;
        /// <summary>
        /// Gets or sets the CheckedSerie.
        /// </summary>
        /// <value>
        /// The CheckedSerie.
        /// </value>
        public object CheckedSerie
        {
            get { return _checkedSerie; }
            set
            {
                if (_checkedSerie != value)
                {
                    _checkedSerie = value;
                    OnPropertyChanged(() => this.CheckedSerie);
                }
            }
        }
        /// <summary>
        /// Todo Context fixen
        /// </summary>
        public ObservableCollection<IFilterEntry> SerienFilter { get; private set; }
        /// <summary>
        /// Gets or sets the KatIds.
        /// </summary>
        /// <value>
        /// The KatIds.
        /// </value>
        public List<int> KatIds { get { return _katIds; } }
        public List<int> SerienIds { get { return _serienIds; } }
        public List<int> MarkierungenIds { get { return _markierungenIds; } }
        /// <summary>
        /// Gets or sets the KategorienFilter.
        /// </summary>
        /// <value>
        /// Todo The KategorienFilter.
        /// </value>
        public ObservableCollection<IFilterEntry> KategorienFilter { get; private set; }
        //public IEnumerable<Kategorie> SortedKategorienListe { get { return Context.Kategoriens.OrderByDescending(x => x.IsAssignedToArtikel).ThenBy(x => x.Name); } }
        //public IEnumerable<Kategorie> AssignedKategorienListe
        //{
        //    get
        //    {
        //        return Context.Kategoriens.OrderBy(x => x.Name).Where(x => x.IsAssignedToArtikel).ToList();
        //    }
        //}
        public ObservableCollection<IFilterEntry> MarkierungenFilter { get; private set; }
        //public IEnumerable<MarkierungDto> SortedMarkierungenListe { get { return Context.Markierungens.OrderByDescending(x => x.IsAssignedToArtikel).ThenBy(x => x.Titel); } }
        //public IEnumerable<MarkierungDto> AssignedMarkierungenliste
        //{
        //    get
        //    {
        //        return Context.Markierungens.OrderBy(x => x.Titel).Where(x => x.IsAssignedToArtikel).ToList(); ;
        //    }
        //}
        public int LoadSize { get { return 100; } }
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
                    OnPropertyChanged(nameof(SuchString));
                }
            }
        }

        private IFilterEntry _SelectedSerienFilter;
        /// <summary>
        /// Gets or sets the SelectedSerienFilterEntry.
        /// </summary>
        /// <value>
        /// The SelectedSerienFilterEntry.
        /// </value>
        public IFilterEntry SelectedSerienFilterEntry
        {
            get { return _SelectedSerienFilter; }
            set
            {
                if (_SelectedSerienFilter != value)
                {
                    _SelectedSerienFilter = value;
                    OnPropertyChanged(nameof(SelectedSerienFilterEntry));
                }
            }
        }

        private IFilterEntry _selectedMarkierungFilterEntry;
        /// <summary>
        /// Gets or sets the SelectedMarkierungFilterEntry.
        /// </summary>
        /// <value>
        /// The SelectedMarkierungFilterEntry.
        /// </value>
        public IFilterEntry SelectedMarkierungFilterEntry
        {
            get { return _selectedMarkierungFilterEntry; }
            set
            {
                if (_selectedMarkierungFilterEntry != value)
                {
                    _selectedMarkierungFilterEntry = value;
                    OnPropertyChanged(nameof(SelectedMarkierungFilterEntry));
                }
            }
        }

        private bool _markierungInverted;
        /// <summary>
        /// Gets or sets the MarkierungInverted.
        /// </summary>
        /// <value>
        /// The MarkierungInverted.
        /// </value>
        public bool MarkierungInverted
        {
            get { return _markierungInverted; }
            set
            {
                if (_markierungInverted != value)
                {
                    _markierungInverted = value;
                }
            }
        }

        //public override AvDBaseEntities Context
        //{
        //    get
        //    {
        //        if (_ctx == null)
        //        {
        //            _ctx = new AvDBaseEntities();
        //        }
        //        return _ctx;
        //    }
        //}
        #region Commands
        public DelegateCommand<object> AddArtikelToClipboard { get; private set; }
        public DelegateCommand<object> AddVariante { get; private set; }
        public DelegateCommand<object> RemoveVariante { get; private set; }
        public DelegateCommand<object> SubmitChanges { get; private set; }
        public DelegateCommand<object> RejectChanges { get; private set; }
        public DelegateCommand<object> SendArtikelOrVariantToBestellung { get; private set; }
        public DelegateCommand SearchArticlesCommand { get; private set; }
        public DelegateCommand ResetFiltersCommand { get; private set; }
        public DelegateCommand LoadMoreArticlesCommand { get; private set; }
        public ICommand ReloadData
        {
            get { return _reloadData; }
            set
            {
                if (_reloadData != value)
                {
                    _reloadData = value;
                }
            }
        }
        #endregion
        /// <summary>
        /// Gets or sets the SelectedVariante.
        /// </summary>
        /// <value>
        /// The SelectedVariante.
        /// </value>
        public ArtikelVariante SelectedVariante
        {
            get { return _selectedVariante; }
            set
            {
                if (_selectedVariante != value)
                {
                    _selectedVariante = value;
                    OnPropertyChanged(() => this.SelectedVariante);
                    CheckCommandsState();
                }
            }
        }
        /// <summary>
        /// Gets or sets the SelectedArtikel.
        /// </summary>
        /// <value>
        /// The SelectedArtikel.
        /// </value>
        //public Artikel SelectedArtikel
        //{
        //    get { return _selectedArtikel; }
        //    set
        //    {
        //        if (_selectedArtikel != value)
        //        {
        //            _selectedArtikel = value;
        //            OnPropertyChanged(() => this.HasVariants);
        //            if (_selectedArtikel != null)
        //            {
        //                if (_selectedArtikel.Varianten.Count > 0)
        //                {
        //                    SelectedVariante = _selectedArtikel.Varianten.ElementAt(0);
        //                }
        //            }
        //            PrepareKategorien();
        //            PrepareMarkierungen();
        //            CheckCommandsState();
        //            OnPropertyChanged(() => SelectedArtikel);
        //        }
        //    }
        //}
        /// <summary>
        /// Gets or sets the SelectedSerienDec.
        /// </summary>
        /// <value>
        /// The SelectedSerienDEc.
        /// </value>
        public SerieDto SelectedSerienDec
        {
            get { return _selectedSerienDec; }
            set
            {
                if (_selectedSerienDec != value)
                {
                    _selectedSerienDec = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the HasVariants.
        /// </summary>
        /// <value>
        /// The HasVariants.
        /// </value>
        //public bool HasVariants
        //{
        //    get { return SelectedArtikel != null ? SelectedArtikel.Varianten.Count > 0 : false; }
        //}
        #region Commands
        public ICommand StartEditVarianteCommand { get; set; }
        public ICommand CancelArtikelEdit { get; set; }
        #endregion
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ArtikelViewModel class.
        /// </summary>
        public ArtikelSearchPageViewModel()
        {
            //AddVariante = new DelegateCommand<object>(AddVarianteAction, CanAddVariante);
            //RemoveVariante = new DelegateCommand<object>(RemoveVarianteAction, CanRemoveVariante);
            FilterSetzen = new DelegateCommand<object>(FilterSetzenAction);
            FilterReset = new DelegateCommand<object>(FilterResetAction);
            SearchArticlesCommand = new DelegateCommand(SearchArticlesAction);
            ResetFiltersCommand = new DelegateCommand(ResetFilterAction);
            LoadMoreArticlesCommand = new DelegateCommand(LoadMoreArticlesAction);
            //SendArtikelOrVariantToBestellung = new DelegateCommand<object>(SendArtikelOrVariantToBestellungAction, x => { return BestellungIsEditing; });
            //AddArtikelToClipboard = new DelegateCommand<object>(AddArtikelToClipboardAction, x => { return SelectedArtikel != null; });
            AddCommand(AddVariante);
            AddCommand(RemoveVariante);
            AddCommand(SubmitChanges);
            AddCommand(RejectChanges);
            AddCommand(FilterSetzen);
            AddCommand(FilterReset);
            AddCommand(AddArtikelToClipboard);
            //_clipboardMgr = ServiceLocator.Current.GetInstance<ClipboardManager>();
            //MessengerInstance.Register<BestellungEditMessage>(this, OnBestellungChanged);
            //MessengerInstance.Register<PubSubEvent<Markierungen>>(this, OnMarkierungenChanged);
            //MessengerInstance.Register<MarkierungDeletedMessage>(this, OnMarkierungenChanged);
            _executeFilterTimer = new System.Threading.Timer(RaiseFilterCallback);
        }

        #endregion Constructors

        #region Public methods
        public void PageUnloaded()
        {
        }
        public void PageLoaded()
        {
            //Context.Load(Context.GetKategorienQuery(), LoadKatgorienCallback, null);
            //Context.Load(Context.GetSerienQuery(), LoadSerienCallback, null);
            LoadMarkierungen();
            //ArtikelListe = new ObservableCollection<Artikel>(Context.GetArtikelPaged(1, 20));
        }
        public void ResetSuchfeld()
        {
            this.SuchString = string.Empty;
            OnPropertyChanged(() => SuchString);
        }
        #endregion Public methods
        #region Private helper methods
        #region Callbacks/Event handler
        //private void OnBestellungChanged(BestellungEditMessage payload)
        //{
        //    BestellungIsEditing = payload.IsEditing;
        //    SendArtikelOrVariantToBestellung.RaiseCanExecuteChanged();
        //}
        private void OnMarkierungenChanged(PubSubEvent<MarkierungDto> msg)
        {
            //Context.Markierungens.Clear();
            //Context.Load(Context.GetMarkierungenByEntityTypeQuery(1), LoadBehavior.RefreshCurrent, LoadMarkierungenCallback, null);
        }

        void serie_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected4Filter")
            {
                //OnPropertyChanged(() => this.SortedSerienList);
                CheckedSerie = sender;
            }
        }
        void kategorie_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            KategorieDto dec = sender as KategorieDto;
            //if (SelectedArtikel == null || dec == null)
            //{
            //    return;
            //}
            if (e.PropertyName == "IsSelected")
            {
                //OnPropertyChanged(() => this.KategorienFilter);
                CheckedKategorie = sender;
            }
            else if (e.PropertyName == "IsAssignedToArtikel")
            {
                //Kategorien_x_Artikel kaPers = SelectedArtikel.Kategorien.FirstOrDefault(x => x.KategorieId == dec.KategorieId);
                //if (dec.IsAssignedToArtikel && kaPers == null)
                //{
                //    kaPers = new Kategorien_x_Artikel();
                //    kaPers.KategorieId = dec.KategorieId;
                //    SelectedArtikel.Kategorien.Add(kaPers);
                //}
                //else if (!dec.IsAssignedToArtikel && kaPers != null)
                //{
                //    SelectedArtikel.Kategorien.Remove(kaPers);
                //}
                //CheckCommandsState();
            }
        }
        void markierung_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MarkierungDto dec = sender as MarkierungDto;
            //if (SelectedArtikel == null || dec == null)
            //    return;
            if (e.PropertyName == "IsSelected")
            {
                //OnPropertyChanged(() => this.MarkierungenFilter);
                CheckedMarkierung = sender;
            }
            else if (e.PropertyName == "IsAssignedToArtikel")
            {
                //Markierungen_x_Artikel mxa = SelectedArtikel.Markierungen.FirstOrDefault(x => x.MarkierungId == dec.MarkierungId);
                //if (dec.IsAssignedToArtikel && mxa == null)
                //{
                //    mxa = new Markierungen_x_Artikel();
                //    mxa.MarkierungId = dec.MarkierungId;
                //    SelectedArtikel.Markierungen.Add(mxa);
                //}
                //else if (!dec.IsAssignedToArtikel && mxa != null)
                //{
                //    SelectedArtikel.Markierungen.Remove(mxa);
                //}
                CheckCommandsState();
            }
            //OnPropertyChanged(() => this.ArtikelHasChanges);
        }
        private void RaiseFilterCallback(object obj)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(() => FilterSetzenAction(null));
        }
        #endregion
        private void LoadMarkierungen()
        {
            //Context.Load(Context.GetMarkierungenByEntityTypeQuery(1), LoadMarkierungenCallback, null);
        }
        /// <summary>
        /// Setze den Kontext auf NULL damit er neu geladen werden kann. Die Serien haben sich geändert.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        //private void PrepareKategorien()
        //{
        //    if (SelectedArtikel == null)
        //        return;
        //    foreach (Kategorien item in Context.Kategoriens)
        //    {
        //        item.SetIsAssigned(SelectedArtikel.Kategorien.FirstOrDefault(x => x.KategorieId == item.KategorieId) != null);
        //    }
        //    OnPropertyChanged(() => this.SortedKategorienListe);
        //    OnPropertyChanged(() => this.AssignedKategorienListe);
        //}
        //private void PrepareMarkierungen()
        //{
        //    if (SelectedArtikel == null)
        //        return;
        //    foreach (Markierungen item in Context.Markierungens)
        //    {
        //        item.SetIsAssigned(SelectedArtikel.Markierungen.FirstOrDefault(x => x.MarkierungId == item.MarkierungId) != null);
        //    }
        //    OnPropertyChanged(() => this.SortedMarkierungenListe);
        //    OnPropertyChanged(() => this.AssignedMarkierungenliste);
        //}
        //private void AddVarianteAction(object obj)
        //{
        //    if (SelectedArtikel != null && SelectedArtikel.Varianten != null)
        //    {
        //        ArtikelVariante av = new ArtikelVariante();
        //        av.ArtikelNr = Guid.NewGuid().ToString().Substring(0, 6);
        //        SelectedArtikel.AddVariante(av);
        //        SelectedVariante = av;
        //        if (StartEditVarianteCommand != null)
        //        {
        //            StartEditVarianteCommand.Execute(SelectedVariante);
        //        }
        //    }
        //}
        //private void RemoveVarianteAction(object obj)
        //{
        //    if (SelectedArtikel != null && SelectedArtikel.Varianten != null && SelectedVariante != null)
        //    {
        //        ArtikelVariante av = SelectedVariante;
        //        SelectedVariante = null;
        //        SelectedArtikel.RemoveVariante(av);
        //    }
        //}
        private void SearchArticlesAction()
        {
            _page = 1;
            ArtikelListe.Clear();
            GatherFilterCriteria();
            LoadArticlesPage();
        }
        private async void LoadArticlesPage()
        {
            var result = await _dataProvider.SearchArticles(_katIds, _serienIds, _markierungenIds, MarkierungInverted, SuchString, _pageSize, _page);
            AddArticlesToList(result);
        }
        private async void AddArticlesToList(List<Artikel> list)
        {
            foreach (Artikel artikel in list)
            {
                ArtikelViewModel vm = new ArtikelViewModel(artikel);
                vm.Markierungen = await _markierungenDataProvider.ListMarkierungenByArtikel(vm.ArtikelId);
                ArtikelListe.Add(vm);
            }
        }
        private void ResetFilterAction()
        {
            SerienFilter.ToList().ForEach(x => x.IsSelected = false);
            KategorienFilter.ToList().ForEach(x => x.IsSelected = false);
            MarkierungenFilter.ToList().ForEach(x => x.IsSelected = false);
            SuchString = null;
            GatherFilterCriteria();
        }
        private void LoadMoreArticlesAction()
        {
            _page++;
            LoadArticlesPage();
        }
        //protected override void FilterResetAction(object obj)
        //{
        //    KategorienFilter.ToList().ForEach(x => x.IsSelected = false);
        //    SortedSerienList.ToList().ForEach(x => x.IsSelected4Filter = false);
        //    MarkierungenFilter.ToList().ForEach(x => x.IsSelected = false);
        //    MarkierungInverted = false;
        //    _katIds = null;
        //    _serienIds = null;
        //    _markierungenIds = null;
        //    _suchString = string.Empty;
        //    _sucheArtikelNr = string.Empty;
        //    RaiseFilterChanged();
        //}
        private bool CanAddVariante(object obj)
        {
            return true;
        }
        private bool CanRemoveVariante(object obj)
        {
            return SelectedVariante != null;
        }
        //private void AddArtikelToClipboardAction(object obj)
        //{
        //    if (SelectedArtikel != null)
        //    {
        //        _clipboardMgr.AddObjectToClipboard(SelectedArtikel);
        //    }
        //}
        /// <summary>
        /// Gets the serien children recursively.
        /// </summary>
        /// <param name="serieBase">The serie base.</param>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        //private void GetSerienChildrenRecursively(Serien root, Serien mother)
        //{
        //    if (mother == null)
        //    {
        //        return;
        //    }
        //    var children = Context.Seriens.Where(x => x.Parent.HasValue && x.Parent.Value == mother.SerienId && !root.IdsInclChildren.Contains(x.SerienId));
        //    foreach (Serien serie in children)
        //    {
        //        root.AddKindId(serie.SerienId);
        //        GetSerienChildrenRecursively(root, serie);
        //    }
        //}
        //private void SendArtikelOrVariantToBestellungAction(object obj)
        //{
        //    if (obj is Artikel)
        //    {
        //        MessengerInstance.Send<PubSubEvent<Artikel>>(new PubSubEvent<Artikel>(obj as Artikel, null));
        //    }
        //    else if (obj is ArtikelVarianten)
        //    {
        //        MessengerInstance.Send<PubSubEvent<ArtikelVarianten>>(new PubSubEvent<ArtikelVarianten>(obj as ArtikelVarianten, null));
        //    }
        //}
        private void BeginFiltering(int dueTime = 200)
        {
            _executeFilterTimer.Change(dueTime, Timeout.Infinite);
        }
        private void GatherFilterCriteria()
        {
            _katIds = KategorienFilter.Where(x => x.IsSelected).Select(x => x.ID).ToList();
            _serienIds = SelectedSerienFilterEntry?.ID >= 0 ? new List<int>() { SelectedSerienFilterEntry.ID } : null;
            _markierungenIds = SelectedMarkierungFilterEntry?.ID >= 0 ? new List<int>() { SelectedMarkierungFilterEntry.ID } : null;
           
        }
        #endregion Private helper methods
    }
}