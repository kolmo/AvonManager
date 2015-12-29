using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AvonManager.KundenHefte.ViewModels
{
    public class KundenSearchViewModel : ErrorAwareBaseViewModel
    {
        #region Private fields
        private const string LOAD = "LOAD";
        IKundenDataProvider _dataProvider;
        private readonly IRegionManager _regionManager;
        ICustomerSearchCriteria _customercriteria;
        #endregion
        public KundenSearchViewModel() { }
        public KundenSearchViewModel(IKundenDataProvider provider, IRegionManager regionManager,
                    ICustomerSearchCriteria customercriteria,
                    BusyFlagsManager busyFlagsManager):base(busyFlagsManager)
        {
            _dataProvider = provider;
            _regionManager = regionManager;
            _customercriteria = customercriteria;
            StartSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
            SelectInitialCommand = new DelegateCommand<string>(LoadCustomersByInital);
        }
        #region Properties
        public ObservableCollection<string> InitialSelectors { get; set; } = new ObservableCollection<string>();
        public InteractionRequest<DeleteEntityConfirmation<KundeViewModel>> DeleteEntityRequest { get; } = new InteractionRequest<DeleteEntityConfirmation<KundeViewModel>>();
        public ICommand StartSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        public ICustomerSearchCriteria Criteria { get { return _customercriteria; } }

        public ObservableCollection<KundeViewModel> AlleKunden { get; set; } = new ObservableCollection<KundeViewModel>();

        private ICommand _selectInitialCommand;
        /// <summary>
        /// Gets or sets the IsSelected.
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public ICommand SelectInitialCommand
        {
            get { return _selectInitialCommand; }
            set
            {
                SetProperty(ref _selectInitialCommand, value);
            }
        }
        #endregion
        #region Public Methods
        public async void LoadData()
        {
            var initials = await _dataProvider.GetAllCustomerInitials();
            InitialSelectors.Clear();
            InitialSelectors.Add("#");
            initials.ForEach(x => InitialSelectors.Add(x));
            LoadCustomersByInital(InitialSelectors[0]);
        }
        #endregion

        #region Private methods
        private void LoadCustomersByInital(string initial)
        {
            ResetSearchAction();
            _customercriteria.Initial = initial!= "#" ? initial : null;
            StartSearch();
        }
        private async void StartSearch()
        {
            BusyFlagsMgr.ResetAllBusyFlags();
            BusyFlagsMgr.IncBusyFlag(LOAD);
            var result = await _dataProvider.SearchKunden(_customercriteria);
            AlleKunden.Clear();
            foreach (KundeDto heft in result)
            {
                KundeViewModel vm = new KundeViewModel(heft, EditKundeAction, DeleteCustomerAction);
                AlleKunden.Add(vm);
            }
            BusyFlagsMgr.DecBusyFlag(LOAD);
        }
        private void ResetSearchAction()
        {
            AlleKunden.Clear();
        }
        private void EditKundeAction(KundeViewModel customer)
        {

            NavigationParameters pars = new NavigationParameters();
            pars.Add("customers", customer);
            var moduleAWorkspace = new Uri("KundenEditView", UriKind.Relative);
            _regionManager.RequestNavigate("KundenDetailsRegion", moduleAWorkspace, pars);
        }
        private void DeleteCustomerAction(KundeViewModel customer)
        {
            DeleteEntityConfirmation<KundeViewModel> deleteConfirmation = new DeleteEntityConfirmation<KundeViewModel>(customer) { Title = "Nachfrage", Content = $"Soll der Kunde '{customer.Nachname}' wirklich gelöscht werden?" };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteCustomerFromDb);
        }
        private void DeleteCustomerFromDb(DeleteEntityConfirmation<KundeViewModel> confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                KundeViewModel customer = confirmation.EntityToDelete;
                int listPosition = AlleKunden.IndexOf(customer);
                int listLength = AlleKunden.Count;
                _dataProvider.DeleteKunde(customer.KundeId);
                AlleKunden.Remove(customer);
                if (listPosition + 1 < listLength)
                {
                    EditKundeAction(AlleKunden[listPosition]);
                }
                else if (AlleKunden.Any())
                {
                    EditKundeAction(AlleKunden.Last());
                }
                else
                {
                    EditKundeAction(null);
                }
            }
        }
        #endregion
    }
}
