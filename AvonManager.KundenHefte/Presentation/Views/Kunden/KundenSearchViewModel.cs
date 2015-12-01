using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Interfaces;
using AvonManager.KundenHefte.Common;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvonManager.KundenHefte.ViewModels
{
    public class KundenSearchViewModel : BindableBase
    {
        #region Private fields
        IKundenDataProvider _dataProvider;
        private readonly IRegionManager _regionManager;
        ICustomerSearchCriteria _customercriteria;
        #endregion
        public KundenSearchViewModel() { }
        public KundenSearchViewModel(IKundenDataProvider provider, IRegionManager regionManager,
                    ICustomerSearchCriteria customercriteria)
        {
            _dataProvider = provider;
            _regionManager = regionManager;
            _customercriteria = customercriteria;
            StarSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
        }
        #region Properties
        public ObservableCollection<InitialSelektor> InitialSelectors { get; } = new ObservableCollection<InitialSelektor>();
        public InteractionRequest<DeleteEntityConfirmation<KundeViewModel>> DeleteEntityRequest { get; } = new InteractionRequest<DeleteEntityConfirmation<KundeViewModel>>();
        public ICommand StarSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
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
        public ICustomerSearchCriteria Criteria { get { return _customercriteria; } }
      
        public ObservableCollection<KundeViewModel> AlleKunden { get; set; } = new ObservableCollection<KundeViewModel>();

        #endregion
        #region Public Methods
        public async void LoadData()
        {
            var initials = await _dataProvider.GetAllCustomerInitials();
            InitialSelectors.Clear();
            initials.ForEach(x => InitialSelectors.Add(new InitialSelektor { Initial = x, SelectInitialCommand = new DelegateCommand<string>(LoadCustomersByInital) }));
            _customercriteria.ActiveCustomersOnly = true;
            _customercriteria.InActiveCustomersOnly = false;
        }
        #endregion

        #region Private methods
        private void LoadCustomersByInital(string initial)
        {
            ResetSearchAction();
            _customercriteria.Initial = initial;
            StartSearch();
        }
        private async void StartSearch()
        {
            var result = await _dataProvider.SearchKunden(_customercriteria);
            AlleKunden.Clear();
            foreach (KundeDto heft in result)
            {
                KundeViewModel vm = new KundeViewModel(heft, EditKundeAction, DeleteCustomerAction);
                AlleKunden.Add(vm);
            }
            SelectedTab = 0;
        }
        private void ResetSearchAction()
        {
            AlleKunden.Clear();
            _customercriteria.Reset();
            _customercriteria.ActiveCustomersOnly = true;
            _customercriteria.InActiveCustomersOnly = false;
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
