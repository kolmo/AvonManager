using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System.Runtime.CompilerServices;
using System.Linq;
using AvonManager.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.ArtikelModule.Notifications;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using AvonManager.Common.Helpers;
using AvonManager.ArtikelModule.Views;
using AvonManager.Common.Base;
using Microsoft.Practices.Prism.PubSubEvents;
using AvonManager.Common.Events;
using System;
using System.Threading.Tasks;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArtikelDetailsViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private const string LOAD = "LOAD";
        private struct BackingFields
        {
            public int ArtikelId;
            public string Name;
            public string Beschreibung;
            public string Nummer;
            public string Inhalt;
            public decimal? Einzelpreis;
            public int? Lagerbestand;
            public int? SerienId;
            public byte[] Bild;
        }
        private BackingFields bFields;
        private BackingFields clone;
        private string resultMessage;
        private ArtikelDto _artikel;
        private bool _isInitializing = false;
        private List<MarkierungDto> _allMarkings;
        private List<KategorieDto> _allCategories;
        IArtikelDataProvider _dataProvider;
        IMarkierungenDataProvider _markierungenDataProvider;
        ISerienDataProvider _seriendataProvider;
        IKategorieProvider _kategorienProvider;
        #region Constructors
        public ArtikelDetailsViewModel() { }
        public ArtikelDetailsViewModel(IArtikelDataProvider dataProvider
            , IMarkierungenDataProvider markierungenDataProvider
            , ISerienDataProvider seriendataProvider
            , IKategorieProvider kategProvider
            , IEventAggregator eventAggregator
            , BusyFlagsManager bFlagsManager) : base(bFlagsManager, eventAggregator)
        {
            _dataProvider = dataProvider;
            _markierungenDataProvider = markierungenDataProvider;
            _seriendataProvider = seriendataProvider;
            _kategorienProvider = kategProvider;
            RaiseMarkierungenSelectionCommand = new DelegateCommand(RaiseMarkierungenSelection);
            RaiseKategorienSelectionCommand = new DelegateCommand(RaiseKategorienSelection);
            EventAggregator.GetEvent<SeriesChangedEvent>().Subscribe(UpdateSeriesList);
        }

        #endregion
        #region Properties

        public InteractionRequest<AssignmentSelectionNotification> ArtikelAssignmentSelectionRequest { get; } = new InteractionRequest<AssignmentSelectionNotification>();
        public List<ArticleMarkingViewModel> ArticleMarkingSelectionList { get; } = new List<ArticleMarkingViewModel>();
        public List<ArticleMarkingViewModel> ArticleMarkingAssignments { get { return ArticleMarkingSelectionList.Where(x => x.IsAssigned).ToList(); } }
        public List<ArticleCategoryViewModel> ArticleCategorySelectionList { get; } = new List<ArticleCategoryViewModel>();
        public List<ArticleCategoryViewModel> ArticleCategoryAssignments { get { return ArticleCategorySelectionList.Where(x => x.IsAssigned).ToList(); } }
        public ObservableCollection<SeriesListEntryViewModel> AlleSerien { get; } = new ObservableCollection<SeriesListEntryViewModel>();
        public ObservableCollection<CategoryListEntryViewModel> Kategorien { get; } = new ObservableCollection<CategoryListEntryViewModel>();
        /// <summary>
        /// Gets or sets the Beschreibung.
        /// </summary>
        /// <value>
        /// The Beschreibung.
        /// </value>
        public string Beschreibung
        {
            get { return bFields.Beschreibung; }
            set
            {
                SetProperty(ref bFields.Beschreibung, value);
            }
        }

        /// <summary>
        /// Gets or sets the ArtikelId.
        /// </summary>
        /// <value>
        /// The ArtikelId.
        /// </value>
        public int ArtikelId
        {
            get { return bFields.ArtikelId; }
            set
            {
                SetProperty(ref bFields.ArtikelId, value);
            }
        }

        /// <summary>
        /// Gets or sets the Inhalt.
        /// </summary>
        /// <value>
        /// The Inhalt.
        /// </value>
        public string Inhalt
        {
            get { return bFields.Inhalt; }
            set
            {
                SetProperty(ref bFields.Inhalt, value);
            }
        }

        /// <summary>
        /// Gets or sets the Einzelpreis.
        /// </summary>
        /// <value>
        /// The Einzelpreis.
        /// </value>
        public decimal? Einzelpreis
        {
            get { return bFields.Einzelpreis; }
            set
            {
                SetProperty(ref bFields.Einzelpreis, value);
            }
        }

        /// <summary>
        /// Gets or sets the Lagerbestand.
        /// </summary>
        /// <value>
        /// The Lagerbestand.
        /// </value>
        public int? Lagerbestand
        {
            get { return bFields.Lagerbestand; }
            set
            {
                SetProperty(ref bFields.Lagerbestand, value);
            }
        }

        /// <summary>
        /// Gets or sets the SerienId.
        /// </summary>
        /// <value>
        /// The SerienId.
        /// </value>
        public int? SerienId
        {
            get { return bFields.SerienId; }
            set
            {
                SetProperty(ref bFields.SerienId, value);
            }
        }

        /// <summary>
        /// Gets or sets the Bild.
        /// </summary>
        /// <value>
        /// The Bild.
        /// </value>
        public byte[] Bild
        {
            get { return bFields.Bild; }
            set
            {
                SetProperty(ref bFields.Bild, value);
            }
        }

        /// <summary>
        /// Gets or sets the Nummer.
        /// </summary>
        /// <value>
        /// The Nummer.
        /// </value>
        public string Nummer
        {
            get { return bFields.Nummer; }
            set
            {
                SetProperty(ref bFields.Nummer, value);
            }
        }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Artikelname
        {
            get { return bFields.Name; }
            set
            {
                SetProperty(ref bFields.Name, value);
            }
        }
        public ICommand RaiseMarkierungenSelectionCommand { get; }
        public ICommand RaiseKategorienSelectionCommand { get; }
        public string InteractionResultMessage
        {
            get
            {
                return this.resultMessage;
            }
            set
            {
                this.resultMessage = value;
                this.OnPropertyChanged("InteractionResultMessage");
            }
        }
        #endregion

        #region Private Props

        #endregion
        #region Public Methods
        public void LoadData()
        {
            Initialize();
        }
        public async void Initialize()
        {
            if (!AlleSerien.Any())
            {
                try
                {
                    var serien = await _seriendataProvider.ListAllSerien();
                    AlleSerien.Clear();
                    AlleSerien.Add(new SeriesListEntryViewModel(null));
                    if (serien != null)
                    {
                        serien.OrderBy(x => x.Name).ToList().ForEach(x => AlleSerien.Add(new SeriesListEntryViewModel(x)));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        #endregion
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
                SaveArtikel();
            return ok;
        }
        #region Private Methods
        private void InitProperties()
        {
            ArtikelId = _artikel.ArtikelId;
            Beschreibung = _artikel.Beschreibung;
            Bild = _artikel.Bild;
            Einzelpreis = _artikel.Einzelpreis;
            Inhalt = _artikel.Inhalt;
            Lagerbestand = _artikel.Lagerbestand;
            Artikelname = _artikel.Name;
            Nummer = _artikel.Nummer;
            SerienId = _artikel.SerienId;
            clone = bFields;
        }
        private void SaveArtikel()
        {
            if (_artikel != null)
            {
                _artikel.Beschreibung = Beschreibung;
                _artikel.Bild = Bild;
                _artikel.Einzelpreis = Einzelpreis;
                _artikel.Inhalt = Inhalt;
                _artikel.Lagerbestand = Lagerbestand;
                _artikel.Name = Artikelname;
                _artikel.Nummer = Nummer;
                _artikel.SerienId = SerienId;
                try
                {
                    _dataProvider.SaveArtikel(_artikel);
                    EventAggregator.GetEvent<ArticleChangedEvent>().Publish(new ArticleChangedEventArgs { Article = _artikel, ChangedType = ChangedType.Update });
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private async void LoadArtikel(int? artikelId)
        {
            if (artikelId.HasValue)
            {
                BusyFlagsMgr.IncBusyFlag(LOAD);
                _isInitializing = true;
                try
                {
                    _artikel = await _dataProvider.LoadArtikel(artikelId.Value);
                    LoadMarkierungen();
                    LoadKategorien();
                    InitProperties();
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
                finally
                {
                    BusyFlagsMgr.DecBusyFlag(LOAD);
                    _isInitializing = false;
                }
            }
        }
        private async void LoadMarkierungen()
        {
            try
            {
                List<ArtikelMarkierungenDto> articelAssignments = await _markierungenDataProvider.ListMarkierungenByArtikel(_artikel.ArtikelId);
                _allMarkings = await _markierungenDataProvider.ListAllMarkierungen(EntitaetDto.Artikel);
                ArticleMarkingSelectionList.Clear();
                foreach (MarkierungDto marking in _allMarkings)
                {
                    bool isAssigned = false;
                    ArtikelMarkierungenDto hkd = articelAssignments.FirstOrDefault(x => x.MarkierungId == marking.MarkierungId);
                    if (hkd != null)
                    {
                        isAssigned = true;
                    }
                    else
                    {
                        hkd = new ArtikelMarkierungenDto { MarkierungId = marking.MarkierungId, ArtikelId = _artikel.ArtikelId };
                    }
                    ArticleMarkingSelectionList.Add(new ArticleMarkingViewModel(_artikel, marking, hkd, _markierungenDataProvider, isAssigned));
                }
                OnPropertyChanged(() => ArticleMarkingAssignments);
                EventAggregator.GetEvent<ArticleChangedEvent>().Publish(new ArticleChangedEventArgs { Article = _artikel, ChangedType = ChangedType.Update });

            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private async void LoadKategorien()
        {
            try
            {
                var kategorien = await _kategorienProvider.ListKategorienByArtikel(_artikel.ArtikelId);
                _allCategories = await _kategorienProvider.ListAllKategorien();
                ArticleCategorySelectionList.Clear();
                foreach (KategorieDto category in _allCategories)
                {
                    bool isAssigned = false;
                    ArticleCategoryDto hkd = kategorien.FirstOrDefault(x => x.CategoryId == category.KategorieId);
                    if (hkd != null)
                    {
                        isAssigned = true;
                    }
                    else
                    {
                        hkd = new ArticleCategoryDto { CategoryId = category.KategorieId, ArtikelId = _artikel.ArtikelId };
                    }
                    ArticleCategorySelectionList.Add(new ArticleCategoryViewModel(_artikel, category, hkd, _kategorienProvider, isAssigned));
                }
                OnPropertyChanged(nameof(ArticleCategoryAssignments));
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            BusyFlagsMgr.ResetBusyflag(LOAD);
            var artikelParameter = navigationContext.Parameters.FirstOrDefault();
            if (artikelParameter.Value is ArticleViewModel)
            {
                int artikelID = ((ArticleViewModel)artikelParameter.Value).ArtikelId;
                LoadArtikel(artikelID);
            }
            else
            {
                // Leere und deaktiviere die View
                BusyFlagsMgr.IncBusyFlag(LOAD);
                _artikel = new ArtikelDto();
                InitProperties();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            ;
        }
        private void UpdateSeriesList(SeriesChangedEventArgs e)
        {
            var series = AlleSerien.FirstOrDefault(x => x.SerienId == e.Series.SerienId);
            if (series!= null)
            {
                series.Name = e.Series.Name;
            }
        }
        private void RaiseMarkierungenSelection()
        {

            AssignmentSelectionNotification notification = new AssignmentSelectionNotification(ArticleMarkingSelectionList);

            notification.Title = "Markierungen";

            this.InteractionResultMessage = "";
            this.ArtikelAssignmentSelectionRequest.Raise(notification,
                returned =>
                {
                    if (returned != null && returned.Confirmed)
                    {
                        LoadMarkierungen();
                    }
                    else
                    {
                        this.InteractionResultMessage = "The user cancelled the operation or didn't select an item.";
                    }
                });
        }
        private void RaiseKategorienSelection()
        {
            AssignmentSelectionNotification notification = new AssignmentSelectionNotification(ArticleCategorySelectionList);

            notification.Title = "Kategorien";

            this.InteractionResultMessage = "";
            this.ArtikelAssignmentSelectionRequest.Raise(notification,
                returned =>
                {
                    if (returned != null && returned.Confirmed)
                    {
                        LoadKategorien();
                    }
                    else
                    {
                        this.InteractionResultMessage = "The user cancelled the operation or didn't select an item.";
                    }
                });
        }
        #endregion
    }
}
