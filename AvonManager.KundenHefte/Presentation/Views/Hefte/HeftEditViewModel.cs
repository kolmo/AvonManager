using AvonManager.BusinessObjects;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using AvonManager.KundenHefte.Presentation;
using AvonManager.KundenHefte.Presentation.Views;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvonManager.KundenHefte.ViewModels
{
    public class HeftEditViewModel : BindableBase, INavigationAware
    {
        private const string LOAD = "LOAD";
        private HeftDto _currentHeft;
        private IHefteDataProvider _dataProvider;
        private bool isInitialized = false;
        IKundenDataProvider _kundenDataProvider;
        ICustomerSearchCriteria _customercriteria;
        BusyFlagsManager _busyFlagsManager;
        public HeftEditViewModel(IHefteDataProvider dataProvider, 
            IKundenDataProvider kundenDataProvider,
            ICustomerSearchCriteria criteria
             , BusyFlagsManager bFlagsManager)
        {
            _busyFlagsManager = bFlagsManager;
            _dataProvider = dataProvider;
            _kundenDataProvider = kundenDataProvider;
            _customercriteria = criteria;
        }

        #region Properties
        public BusyFlagsManager Mgr { get { return _busyFlagsManager; } }
        public ObservableCollection<HeftKundeViewModel> SortedKundenListe { get; private set; } = new ObservableCollection<HeftKundeViewModel>();
        public int HeftId { get { return _currentHeft.HeftId; } }
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

        #endregion

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (isInitialized)
                EndEdit();
            return ok;
        }
        public async void LoadCurrentHeft(int heftId)
        {
            _busyFlagsManager.IncBusyFlag(LOAD);
            isInitialized = false;
            _currentHeft = await _dataProvider.LoadHeft(heftId);
            InitProperties();
            _customercriteria.Reset();
            _customercriteria.GetsBrochure = true;
            var kundenListe = await _kundenDataProvider.SearchKunden(_customercriteria);
            var assignments = await _dataProvider.ListHeftKunden(heftId);
            SortedKundenListe.Clear();
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
                SortedKundenListe.Add(new HeftKundeViewModel(kunde, _currentHeft, hkd, _dataProvider, isAssigned));
            }
            isInitialized = true;
            _busyFlagsManager.DecBusyFlag(LOAD);

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
            _dataProvider.SaveHeft(_currentHeft);
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
            _busyFlagsManager.ResetBusyflag(LOAD);
            var brochureParameter = navigationContext.Parameters.FirstOrDefault();
            if (brochureParameter.Value is HeftViewModel)
            {
                int heftId = ((HeftViewModel)brochureParameter.Value).HeftId;
                LoadCurrentHeft(heftId);
            }
            else
            {
                // Leere und deaktiviere die View
                _busyFlagsManager.IncBusyFlag(LOAD);
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
