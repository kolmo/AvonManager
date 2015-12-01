using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using AvonManager.KundenHefte.Common;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using AvonManager.Interfaces.Criteria;
using AvonManager.Common;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.Common.Base;

namespace AvonManager.KundenHefte.ViewModels
{
    public class HefteSearchViewModel : BindableBase
    {
        #region Private fields
        IHefteDataProvider _dataProvider;
        private ObservableCollection<HeftViewModel> _alleHefte;
        private ObservableCollection<YearSelector> _alleJahre;
        private IRegionManager _regionManager;
        private bool _isDataLoaded = false;
        private readonly IBrochureSearchCriteria _brochureSearchCriteria;
        #endregion
        public HefteSearchViewModel()
        {
        }
        public HefteSearchViewModel(IHefteDataProvider dataProvider, IRegionManager regionManager,
            IBrochureSearchCriteria brochureSearchCriteria)
        {
            _dataProvider = dataProvider;
            _regionManager = regionManager;
            _brochureSearchCriteria = brochureSearchCriteria;
            StarSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
        }
        #region Properties
        public InteractionRequest<DeleteEntityConfirmation<HeftViewModel>> DeleteEntityRequest { get; } = new InteractionRequest<DeleteEntityConfirmation<HeftViewModel>>();
        public IBrochureSearchCriteria Criteria { get { return _brochureSearchCriteria; } }
        private int _selectedTab = 0;
        /// <summary>
        /// Gets or sets the SelectedTab.
        /// </summary>
        /// <value>
        /// The SelectedTab.
        /// </value>
        public int SelectedTab
        {
            get { return _selectedTab; }
            set { SetProperty(ref _selectedTab, value); }
        }

        public ICommand StarSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        /// <summary>
        /// Gets or sets the AlleKategorien.
        /// </summary>
        /// <value>
        /// The AlleKategorien.
        /// </value>
        public ObservableCollection<HeftViewModel> AlleHefte
        {
            get { return _alleHefte ?? (_alleHefte = new ObservableCollection<HeftViewModel>()); }
            set
            {
                if (_alleHefte != value)
                {
                    _alleHefte = value;
                    OnPropertyChanged(() => this.AlleHefte);
                }
            }
        }
        public ObservableCollection<YearSelector> AlleJahre
        {
            get
            {
                return _alleJahre ?? (_alleJahre = new ObservableCollection<YearSelector>());
            }
            set
            {
                if (_alleJahre != value)
                {
                    _alleJahre = value;
                    OnPropertyChanged(nameof(AlleJahre));
                }
            }
        }

        #endregion
        public async void LoadData()
        {
            if (!_isDataLoaded)
            {
                var allyears = await _dataProvider.ListAllYears();
                AlleJahre.Clear();
                foreach (int year in allyears)
                {
                    AlleJahre.Add(new YearSelector { Year = year });
                }
                _isDataLoaded = true;
            }
        }
        private async void StartSearch()
        {
            GatherFilterCriteria();
            var result = await _dataProvider.SearchBrochures(_brochureSearchCriteria);
            AlleHefte.Clear();
            foreach (HeftDto heft in result)
            {
                HeftViewModel vm = new HeftViewModel(heft, EditHeftAction, DeleteBrochureAction);
                AlleHefte.Add(vm);
            }
            OnPropertyChanged(() => this.AlleHefte);
            SelectedTab = 0;
        }
        private void GatherFilterCriteria()
        {
            Criteria.Years = AlleJahre.Where(x => x.IsSelected).Select(x => x.Year).ToArray();
        }
        private void ResetSearchAction()
        {
            AlleHefte.Clear();
            Criteria.Reset();
            AlleJahre.ToList().ForEach(x => x.IsSelected = false);
        }
        private void EditHeftAction(HeftViewModel brochure)
        {
            NavigationParameters pars = new NavigationParameters();
            pars.Add("brochure", brochure);
            var moduleAWorkspace = new Uri("HeftEditView", UriKind.Relative);
            _regionManager.RequestNavigate("HeftDetailsRegion", moduleAWorkspace, pars);
        }
        private void DeleteBrochureAction(HeftViewModel brochure)
        {
            if (brochure != null)
            {
                DeleteEntityConfirmation<HeftViewModel> deleteConfirmation = new DeleteEntityConfirmation<HeftViewModel>(brochure) { Title = "Nachfrage", Content = $"Soll das Heft '{brochure.Titel}' wirklich gelöscht werden?" };
                DeleteEntityRequest.Raise(deleteConfirmation, DeleteBrochureFromDb);
            }
        }
        private void DeleteBrochureFromDb(DeleteEntityConfirmation<HeftViewModel> confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                HeftViewModel brochure = confirmation.EntityToDelete;
                int listPosition = AlleHefte.IndexOf(brochure);
                int listLength = AlleHefte.Count;
                _dataProvider.DeleteHeft(brochure.HeftId);
                AlleHefte.Remove(brochure);
                if (listPosition + 1 < listLength)
                {
                    EditHeftAction(AlleHefte[listPosition]);
                }
                else if (AlleHefte.Any())
                {
                    EditHeftAction(AlleHefte.Last());
                }
                else
                {
                    EditHeftAction(null);
                }
            }
        }
    }
}
