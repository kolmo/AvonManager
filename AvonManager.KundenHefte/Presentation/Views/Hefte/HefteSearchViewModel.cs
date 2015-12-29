using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using System;
using AvonManager.Interfaces.Criteria;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;

namespace AvonManager.KundenHefte.ViewModels
{
    public class HefteSearchViewModel : BindableBase
    {
        #region Private fields
        private const string LOAD = "LOAD";
        IHefteDataProvider _dataProvider;
        private ObservableCollection<HeftViewModel> _alleHefte;
        private IRegionManager _regionManager;
        BusyFlagsManager _busyFlagsManager;
        private readonly IBrochureSearchCriteria _brochureSearchCriteria;
        #endregion
        public HefteSearchViewModel()
        {
        }
        public HefteSearchViewModel(IHefteDataProvider dataProvider, IRegionManager regionManager,
            IBrochureSearchCriteria brochureSearchCriteria,
             BusyFlagsManager busyFlagsManager)
        {
            _busyFlagsManager = busyFlagsManager;
            _dataProvider = dataProvider;
            _regionManager = regionManager;
            _brochureSearchCriteria = brochureSearchCriteria;
            StarSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
            AddBrochureCommand = new DelegateCommand(AddBrochureAction);
        }
        #region Properties
        public BusyFlagsManager Mgr { get { return _busyFlagsManager; } }
        public InteractionRequest<DeleteEntityConfirmation<HeftViewModel>> DeleteEntityRequest { get; } = new InteractionRequest<DeleteEntityConfirmation<HeftViewModel>>();
        public IBrochureSearchCriteria Criteria { get { return _brochureSearchCriteria; } }
        public ICommand StarSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        public DelegateCommand AddBrochureCommand { get; private set; }
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
        #endregion
        public void LoadData()
        {
            StartSearch();
        }
        private async void StartSearch()
        {
            _busyFlagsManager.ResetAllBusyFlags();
            _busyFlagsManager.IncBusyFlag(LOAD);
            var result = await _dataProvider.SearchBrochures(_brochureSearchCriteria);
            AlleHefte.Clear();
            foreach (HeftDto heft in result)
            {
                HeftViewModel vm = new HeftViewModel(heft, EditBrochureAction, DeleteBrochureAction);
                AlleHefte.Add(vm);
            }
            OnPropertyChanged(() => this.AlleHefte);
            _busyFlagsManager.DecBusyFlag(LOAD);
        }

        private void ResetSearchAction()
        {
            AlleHefte.Clear();
            Criteria.Reset();
        }
        private void EditBrochureAction(HeftViewModel brochure)
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
                    EditBrochureAction(AlleHefte[listPosition]);
                }
                else if (AlleHefte.Any())
                {
                    EditBrochureAction(AlleHefte.Last());
                }
                else
                {
                    EditBrochureAction(null);
                }
            }
        }

        private void AddBrochureAction()
        {
            var newBrochure = new HeftDto { Titel = "Neues Heft", Jahr = DateTime.Now.Year };
            newBrochure.HeftId = _dataProvider.AddHeft(newBrochure);
            HeftViewModel vm = new HeftViewModel(newBrochure, EditBrochureAction, DeleteBrochureAction);
            AlleHefte.Insert(0, vm);
            EditBrochureAction(vm);
        }
    }
}
