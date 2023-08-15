using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using AvonManager.KundenHefte.Presentation.Views;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvonManager.KundenHefte.ViewModels
{
    public class HeftEditViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private const string LOAD = "LOAD";
        private HeftDto _currentHeft;
        private IHefteDataProvider _dataProvider;
        private bool isInitialized = false;
        private IKundenDataProvider _kundenDataProvider;
        private ICustomerSearchCriteria _customercriteria;

        public HeftEditViewModel(IHefteDataProvider dataProvider,
            IKundenDataProvider kundenDataProvider,
            ICustomerSearchCriteria criteria,
            IEventAggregator eventAggregator
             , BusyFlagsManager bFlagsManager) : base(bFlagsManager, eventAggregator)
        {
            _dataProvider = dataProvider;
            _kundenDataProvider = kundenDataProvider;
            _customercriteria = criteria;
        }

        #region Properties

        public ObservableCollection<HeftKundeViewModel> KundenListe { get; private set; } = new ObservableCollection<HeftKundeViewModel>();

        private IEnumerable<HeftKundeViewModel> _sortedCustomerList;

        /// <summary>
        /// Gets or sets the SortedCustomerList.
        /// </summary>
        /// <value>
        /// The SortedCustomerList.
        /// </value>
        public IEnumerable<HeftKundeViewModel> SortedCustomerList
        {
            get { return _sortedCustomerList; }
            set { SetProperty(ref _sortedCustomerList, value); }
        }

        public int HeftId
        { get { return _currentHeft.HeftId; } }
        private string _titel;

        public string Titel
        {
            get { return _titel; }
            set
            {
                SetProperty(ref _titel, value);
            }
        }

        private int? _jahr;

        public int? Jahr
        {
            get { return _jahr; }
            set
            {
                SetProperty(ref _jahr, value);
            }
        }

        private string _beschreibung;

        public string Beschreibung
        {
            get { return _beschreibung; }
            set
            {
                SetProperty(ref _beschreibung, value);
            }
        }

        private byte[] _bild;

        public byte[] Bild
        {
            get { return _bild; }
            set
            {
                SetProperty(ref _bild, value);
            }
        }

        #endregion Properties

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (isInitialized)
                EndEdit();
            return ok;
        }

        public async void LoadCurrentHeft(int heftId)
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                isInitialized = false;
                _currentHeft = await _dataProvider.LoadHeft(heftId);
                InitProperties();
                _customercriteria.Reset();
                _customercriteria.GetsBrochure = true;
                var kundenListe = await _kundenDataProvider.SearchKunden(_customercriteria);
                var assignments = await _dataProvider.ListHeftKunden(heftId);
                KundenListe.Clear();
                foreach (KundeDto kunde in kundenListe)
                {
                    bool isAssigned = false;
                    HeftKundeDto hkd = assignments.FirstOrDefault(x => x.KundenId == kunde.KundenId);
                    if (hkd != null)
                    {
                        isAssigned = true;
                    }
                    else
                    {
                        hkd = new HeftKundeDto { KundenId = kunde.KundenId, HeftId = _currentHeft.HeftId };
                    }
                    var hkvm = new HeftKundeViewModel(kunde, _currentHeft, hkd, _dataProvider, isAssigned);
                    KundenListe.Add(hkvm);
                }
                RefreshSorting();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            isInitialized = true;
            BusyFlagsMgr.DecBusyFlag(LOAD);
        }

        private void RefreshSorting()
        {
            SortedCustomerList = KundenListe.OrderBy(x => x.Received).ToList();
        }

        private void InitProperties()
        {
            Titel = _currentHeft.Titel;
            Jahr = _currentHeft.Jahr;
            Beschreibung = _currentHeft.Beschreibung;
            Bild = _currentHeft.Bild;
        }

        private void SaveHeft()
        {
            try
            {
                _dataProvider.SaveHeft(_currentHeft);
                EventAggregator.GetEvent<BrochureChangedEvent>().Publish(new BrochureChangedEventArgs { Brochure = _currentHeft, ChangedType = ChangedType.Update });
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }

        public void BeginEdit()
        {
            ;
        }

        public void EndEdit()
        {
            if (isInitialized)
            {
                _currentHeft.Titel = Titel;
                _currentHeft.Beschreibung = Beschreibung;
                _currentHeft.Jahr = Jahr;
                _currentHeft.Bild = Bild;
                SaveHeft();
            }
        }

        public void CancelEdit()
        {
            ;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            BusyFlagsMgr.ResetBusyflag(LOAD);
            var brochureParameter = navigationContext.Parameters.FirstOrDefault();
            if (brochureParameter.Value is HeftViewModel)
            {
                int heftId = ((HeftViewModel)brochureParameter.Value).HeftId;
                LoadCurrentHeft(heftId);
            }
            else
            {
                // Leere und deaktiviere die View
                BusyFlagsMgr.IncBusyFlag(LOAD);
                _currentHeft = new HeftDto();
                InitProperties();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}