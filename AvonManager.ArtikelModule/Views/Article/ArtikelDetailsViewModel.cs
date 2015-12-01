﻿using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using AvonManager.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.ArtikelModule.Notifications;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using AvonManager.Common.Base;
using AvonManager.ArtikelModule.Common;
using AvonManager.Common.Helpers;
using AvonManager.ArtikelModule.Views;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArtikelDetailsViewModel : BindableBase, INavigationAware
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
        IArtikelDataProvider _dataProvider;
        IMarkierungenDataProvider _markierungenDataProvider;
        ISerienDataProvider _seriendataProvider;
        IKategorieProvider _kategorienProvider;
        BusyFlagsManager _busyFlagsManager;
        #region Constructors
        public ArtikelDetailsViewModel() { }
        public ArtikelDetailsViewModel(IArtikelDataProvider dataProvider
            , IMarkierungenDataProvider markierungenDataProvider
            , ISerienDataProvider seriendataProvider
            , IKategorieProvider kategProvider
            , BusyFlagsManager bFlagsManager)
        {
            _busyFlagsManager = bFlagsManager;
            _dataProvider = dataProvider;
            _markierungenDataProvider = markierungenDataProvider;
            _seriendataProvider = seriendataProvider;
            _kategorienProvider = kategProvider;
            RaiseMarkierungenSelectionCommand = new DelegateCommand(RaiseMarkierungenSelection);
            RaiseKategorienSelectionCommand = new DelegateCommand(RaiseKategorienSelection);
        }

        #endregion
        #region Properties
        public BusyFlagsManager Mgr { get { return _busyFlagsManager; } }

        public InteractionRequest<AssignmentSelectionNotification> ArtikelAssignmentSelectionRequest { get; } = new InteractionRequest<AssignmentSelectionNotification>();
        public List<ArticleMarkingViewModel> ArticleMarkingSelectionList { get; } = new List<ArticleMarkingViewModel>();
        public List<ArticleMarkingViewModel> ArticleMarkingAssignments { get { return ArticleMarkingSelectionList.Where(x => x.IsAssigned).ToList(); } }
        public ObservableCollection<SeriesListEntryViewModel> AlleSerien { get; } = new ObservableCollection<SeriesListEntryViewModel>();
        public ObservableCollection<CategoryViewModel> Kategorien { get; } = new ObservableCollection<CategoryViewModel>();
        public ObservableCollection<ArticleMarkingViewModel> Markierungen { get; } = new ObservableCollection<ArticleMarkingViewModel>();
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
            var serien = await _seriendataProvider.ListAllSerien();
            AlleSerien.Clear();
            AlleSerien.Add(new SeriesListEntryViewModel(null));
            if (serien != null)
            {
                serien.ToList().ForEach(x => AlleSerien.Add(new SeriesListEntryViewModel(x)));
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
                _dataProvider.SaveArtikel(_artikel);
            }
        }
        private async void LoadArtikel(int? artikelId)
        {
            if (artikelId.HasValue)
            {
                _busyFlagsManager.IncBusyFlag(LOAD);
                _isInitializing = true;
                _artikel = await _dataProvider.LoadArtikel(artikelId.Value);
                LoadMarkierungen();
                LoadKategorien();
                InitProperties();
                _isInitializing = false;
                _busyFlagsManager.DecBusyFlag(LOAD);
            }
        }
        private async void LoadMarkierungen()
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
        }
        private async void LoadKategorien()
        {
            var kategorien = await _kategorienProvider.ListKategorienByArtikel(_artikel.ArtikelId);
            Kategorien.Clear();
            kategorien.ForEach(x => Kategorien.Add(new CategoryViewModel(x)));
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _busyFlagsManager.ResetBusyflag(LOAD);
            var artikelParameter = navigationContext.Parameters.FirstOrDefault();
            if (artikelParameter.Value is ArtikelViewModel)
            {
                int artikelID = ((ArtikelViewModel)artikelParameter.Value).ArtikelId;
                LoadArtikel(artikelID);
            }
            else
            {
                // Leere und deaktiviere die View
                _busyFlagsManager.IncBusyFlag(LOAD);
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
        private async void RaiseKategorienSelection()
        {
            // Here we have a custom implementation of INotification which allows us to pass custom data in the 
            // parameter of the interaction request. In this case, we are passing a list of items.
            var markierungen = await _kategorienProvider.ListAllKategorien();
            List<FilterEntryBase> alleKats = new List<FilterEntryBase>();
            foreach (var item in markierungen)
            {
                FilterEntryBase vm = new KategorieFilterEntry(item);
                vm.IsSelected = Kategorien.Any(x => x.KategorieId == item.KategorieId);
                alleKats.Add(vm);
            }

            AssignmentSelectionNotification notification = new AssignmentSelectionNotification(alleKats);
            notification.Title = "Kategorien";

            // The custom popup view in this case has its own view model which implements the IInteractionRequestAware interface
            // therefore, its Notification property will be automatically populated with this notification by the PopupWindowAction.
            // Like this that view model is able to recieve data from this one without knowing each other.
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
        private void UpdateArtikelMarkierung(int markierungId, bool insert)
        {
            //if (insert)
            //    _markierungenDataProvider.AddMarkierungArtikel(_artikel.ArtikelId, markierungId, insert);
            //else
            //    _markierungenDataProvider.DeleteMarkierungArtikel(_artikel.ArtikelId, markierungId, insert);

        }
        private void UpdateArtikelKategorie(int kategorieId, bool insert)
        {
            _kategorienProvider.UpdateKategorieArtikel(_artikel.ArtikelId, kategorieId, insert);
        }
        #endregion
    }
}
