using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.PubSubEvents;
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
        public KundenSearchViewModel(IKundenDataProvider provider,
            IRegionManager regionManager,
            ICustomerSearchCriteria customercriteria,
            IEventAggregator eventAggregator,
            BusyFlagsManager busyFlagsManager) : base(busyFlagsManager, eventAggregator)
        {
            _dataProvider = provider;
            _regionManager = regionManager;
            _customercriteria = customercriteria;
            StartSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
            SelectInitialCommand = new DelegateCommand<string>(LoadCustomersByInital);
            EditKundeCommand = new DelegateCommand<KundeViewModel>(EditKundeAction);
            AddCustomerCommand = new DelegateCommand(AddCustomerAction);
            DeleteCustomerCommand = new DelegateCommand<KundeViewModel>(DeleteCustomerAction, x=> x?.OrderCount == 0);
            EventAggregator.GetEvent<CustomerChangedEvent>().Subscribe(x => StartSearch());
        }
        #region Properties
        public ObservableCollection<string> InitialSelectors { get; set; } = new ObservableCollection<string>();
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public ICommand StartSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        public ICustomerSearchCriteria Criteria { get { return _customercriteria; } }
        public DelegateCommand<KundeViewModel> EditKundeCommand { get; private set; }
        public DelegateCommand<KundeViewModel> DeleteCustomerCommand { get; private set; }
        public DelegateCommand AddCustomerCommand { get; private set; }

        public ObservableCollection<KundeViewModel> AlleKunden { get; set; } = new ObservableCollection<KundeViewModel>();

        private ICommand _selectInitialCommand;
        /// <summary>
        /// Gets or sets the IsSelected.
        /// /// </summary>
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
            try
            {
                var initials = await _dataProvider.GetAllCustomerInitials();
                InitialSelectors.Clear();
                InitialSelectors.Add("#");
                initials.ForEach(x => InitialSelectors.Add(x));
                LoadCustomersByInital(InitialSelectors[0]);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        #endregion

        #region Private methods
        private void LoadCustomersByInital(string initial)
        {
            ResetSearchAction();
            _customercriteria.Initial = initial != "#" ? initial : null;
            StartSearch();
        }
        private async void StartSearch()
        {
            BusyFlagsMgr.ResetAllBusyFlags();
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var result = await _dataProvider.SearchKunden(_customercriteria);
                AlleKunden.Clear();
                foreach (KundeDto customer in result)
                {
                    KundeViewModel vm = new KundeViewModel(customer);
                    AlleKunden.Add(vm);
                }
                var customerIds = result.Select(x => x.KundenId).ToArray();
                var dic = await _dataProvider.ListOrderCountsByCustomer(customerIds);
                if (dic?.Count > 0)
                {
                    foreach (var customerViewModel in AlleKunden)
                    {
                        if (dic.ContainsKey(customerViewModel.KundeId))
                        {
                            customerViewModel.OrderCount = dic[customerViewModel.KundeId];
                        }
                    }
                    DeleteCustomerCommand.RaiseCanExecuteChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            BusyFlagsMgr.DecBusyFlag(LOAD);
        }
        private void ResetSearchAction()
        {
            AlleKunden.Clear();
        }
        private void AddCustomerAction()
        {
            var newCustomer = new KundeDto { Nachname = "Mustermann", Vorname="Erika"};
            newCustomer.KundenId = _dataProvider.AddKunde(newCustomer);
            KundeViewModel vm = new KundeViewModel(newCustomer);
            AlleKunden.Insert(0, vm);
            EditKundeAction(vm);
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
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation()
            {
                Title = "Nachfrage",
                Content = $"Soll der Kunde '{customer.Nachname}' wirklich gelöscht werden?",
                Entity = customer
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteCustomerFromDb);
        }
        private void DeleteCustomerFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                KundeViewModel customer = confirmation.Entity as KundeViewModel;
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
