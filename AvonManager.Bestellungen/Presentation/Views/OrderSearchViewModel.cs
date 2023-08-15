using AvonManager.Bestellungen.Common;
using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AvonManager.Bestellungen.Presentation.Views
{
    public class OrderSearchViewModel : ErrorAwareBaseViewModel
    {
        #region Private fields

        private const string LOAD = "LOAD";
        private IOrderDataProvider _orderDataProvider;
        private readonly IKundenDataProvider _customerDataProvider;
        private readonly ICustomerSearchCriteria _customerSearchCriteria;
        private readonly IOrderSearchCriteria _orderSearchCriteria;
        private string _currentInitial;
        private IRegionManager _regionManager;
        private int _page = 0;

        #endregion Private fields

        #region Constructor

        public OrderSearchViewModel(IOrderDataProvider orderDataProvider,
    IKundenDataProvider customerDataProvider,
    IRegionManager regionManager,
    ICustomerSearchCriteria customerSearchCriteria,
    IOrderSearchCriteria orderSearchCriteria,
    IEventAggregator eventAggregator,
    BusyFlagsManager busyFlagsManager) : base(busyFlagsManager, eventAggregator)
        {
            _orderSearchCriteria = orderSearchCriteria;
            _orderDataProvider = orderDataProvider;
            _customerDataProvider = customerDataProvider;
            _regionManager = regionManager;
            _customerSearchCriteria = customerSearchCriteria;
            StartSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
            SelectInitialCommand = new DelegateCommand<string>(LoadCustomersByInital);
            SelectCommand = new DelegateCommand<CustomerListEntry>(SelectCustomerAction);
            CreateOrderCommand = new DelegateCommand<CustomerListEntry>(CreateNewOrderForCustomerAction);
            EditOrderCommand = new DelegateCommand<OrderViewModel>(EditOrderAction);
            DeleteOrderCommand = new DelegateCommand<OrderViewModel>(DeleteOrderAction);
            _customerSearchCriteria.ActiveCustomersOnly = true;
            _orderSearchCriteria.ActiveCustomersOnly = true;
        }

        #endregion Constructor

        #region Properties

        private ObservableCollection<string> _customerInitialsList;

        /// <summary>
        /// Gets or sets the CustomerInitialsList.
        /// </summary>
        /// <value>
        /// The CustomerInitialsList.
        /// </value>
        public ObservableCollection<string> CustomerInitialsList
        {
            get { return _customerInitialsList; }
            set { SetProperty(ref _customerInitialsList, value); }
        }

        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public InteractionRequest<NewOrderConfirmation> NewOrderRequest { get; } = new InteractionRequest<NewOrderConfirmation>();
        public ICommand StartSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        public ObservableCollection<OrderViewModel> AllOrders { get; } = new ObservableCollection<OrderViewModel>();
        public DelegateCommand<CustomerListEntry> SelectCommand { get; private set; }
        public DelegateCommand<CustomerListEntry> CreateOrderCommand { get; private set; }
        public DelegateCommand<OrderViewModel> EditOrderCommand { get; private set; }
        public DelegateCommand<OrderViewModel> DeleteOrderCommand { get; private set; }
        public ObservableCollection<CustomerListEntry> CustomersWithOrders { get; } = new ObservableCollection<CustomerListEntry>();

        public IOrderSearchCriteria Criteria
        { get { return _orderSearchCriteria; } }

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

        private bool _withInactiveCustomers;

        /// <summary>
        /// Gets or sets the WithInactiveCustomers.
        /// </summary>
        /// <value>
        /// The WithInactiveCustomers.
        /// </value>
        public bool WithInactiveCustomers
        {
            get { return _withInactiveCustomers; }
            set
            {
                SetProperty(ref _withInactiveCustomers, value);
                _customerSearchCriteria.ActiveCustomersOnly = !_withInactiveCustomers;
                _orderSearchCriteria.ActiveCustomersOnly = _customerSearchCriteria.ActiveCustomersOnly;
                LoadCustomersByInital(_currentInitial);
                AllOrders.Clear();
                StartSearch();
            }
        }

        #endregion Properties

        #region Public methods

        public async void LoadSupplementData()
        {
            BusyFlagsMgr.ResetAllBusyFlags();
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                CustomerInitialsList = new ObservableCollection<string>(await _customerDataProvider.GetAllCustomerInitials());
                CustomerInitialsList.Insert(0, "#");
                LoadCustomersByInital(CustomerInitialsList[0]);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            finally
            {
                BusyFlagsMgr.DecBusyFlag(LOAD);
            }
        }

        #endregion Public methods

        #region Private methods

        private async void LoadCustomersByInital(string initial = "#")
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            _currentInitial = initial;
            _customerSearchCriteria.Initial = initial != "#" ? initial : null;
            try
            {
                var customers = await _customerDataProvider.SearchKunden(_customerSearchCriteria);
                CustomersWithOrders.Clear();
                foreach (var customer in customers)
                {
                    var listEntry = new CustomerListEntry(customer);
                    CustomersWithOrders.Add(listEntry);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            finally
            {
                BusyFlagsMgr.DecBusyFlag(LOAD);
            }
        }

        private void StartSearch()
        {
            LoadOrderPage();
        }

        private async void LoadOrderPage()
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            _page++;
            try
            {
                var result = await _orderDataProvider.SearchOrders(_orderSearchCriteria);
                AddOrdersToList(result);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            finally
            {
                BusyFlagsMgr.DecBusyFlag(LOAD);
            }
        }

        private async void AddOrdersToList(List<BestellungDto> orders)
        {
            foreach (BestellungDto bestellung in orders)
            {
                var customer = CustomersWithOrders.FirstOrDefault(x => x.ID == bestellung.KundenId.Value)?.Customer;
                if (customer == null)
                {
                    customer = (await _customerDataProvider.LoadCustomers(new int[] { bestellung.KundenId.Value })).First();
                }
                OrderViewModel vm = new OrderViewModel(bestellung, customer);
                AllOrders.Add(vm);
            }
        }

        private void ResetSearchAction()
        {
            AllOrders.Clear();
            _orderSearchCriteria.Reset();
            _customerSearchCriteria.ActiveCustomersOnly = true;
            SetProperty(ref _withInactiveCustomers, false, nameof(WithInactiveCustomers));
            LoadCustomersByInital();
            _page = 0;
        }

        private void EditOrderAction(OrderViewModel order)
        {
            NavigationParameters pars = new NavigationParameters();
            pars.Add("orders", order);
            var moduleAWorkspace = new Uri("OrderEditView", UriKind.Relative);
            _regionManager.RequestNavigate(AvonManager.Common.RegionNames.OrderDetailsRegion, moduleAWorkspace, pars);
        }

        private void LoadMoreAction()
        {
            _page++;
            LoadOrderPage();
        }

        private void DeleteOrderAction(OrderViewModel order)
        {
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation()
            {
                Title = "Nachfrage",
                Content = $"Soll die Bestellung vom '{order.OrderDate}' wirklich gelöscht werden?",
                Entity = order
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteOrderFromDb);
        }

        private void DeleteOrderFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                OrderViewModel order = confirmation.Entity as OrderViewModel;
                BusyFlagsMgr.IncBusyFlag(LOAD);
                int listPosition = AllOrders.IndexOf(order);
                int listLength = AllOrders.Count;
                try
                {
                    _orderDataProvider.DeleteOrder(order.OrderId);
                    AllOrders.Remove(order);
                    if (listPosition + 1 < listLength)
                    {
                        EditOrderAction(AllOrders[listPosition]);
                    }
                    else if (AllOrders.Any())
                    {
                        EditOrderAction(AllOrders.Last());
                    }
                    else
                    {
                        EditOrderAction(null);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
                finally
                {
                    BusyFlagsMgr.DecBusyFlag(LOAD);
                }
            }
        }

        private void SelectCustomerAction(CustomerListEntry customer)
        {
            AllOrders.Clear();
            _orderSearchCriteria.CustomerIds = new int[] { customer.ID };
            StartSearch();
        }

        private void CreateNewOrderForCustomerAction(CustomerListEntry customer)
        {
            if (customer != null)
            {
                NewOrderConfirmation newOrderConfirmation = new NewOrderConfirmation()
                {
                    Title = "Nachfrage",
                    Content = $"Soll eine neue Bestellung für '{customer.DisplayName}' angelegt werden?",
                    Customer = customer
                };
                NewOrderRequest.Raise(newOrderConfirmation, CreateOrder);
            }
        }

        private void CreateOrder(NewOrderConfirmation confirmation)
        {
            if (confirmation.Confirmed)
            {
                BestellungDto order = new BestellungDto() { KundenId = confirmation.Customer.ID, Datum = DateTime.Now, StatusId = 1 };
                try
                {
                    order.BestellId = _orderDataProvider.AddOrder(order);
                    OrderViewModel vm = new OrderViewModel(order, confirmation.Customer.Customer);
                    AllOrders.Insert(0, vm);
                    EditOrderAction(vm);
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }

        #endregion Private methods
    }
}