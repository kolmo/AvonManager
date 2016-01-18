using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AvonManager.Interfaces;
using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;
using AvonManager.Bestellungen.Common;
using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.Bestellungen.Presentation.Views
{
    public class OrderSearchViewModel : ErrorAwareBaseViewModel
    {
        private const string LOAD = "LOAD";
        private IOrderDataProvider _orderDataProvider;
        private readonly IKundenDataProvider _customerDataProvider;
        private readonly ICustomerSearchCriteria _customerSearchCriteria;
        private readonly IOrderSearchCriteria _orderSearchCriteria;
        IRegionManager _regionManager;
        int _pageSize = 500;
        int _page = 0;
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
            StarSearchCommand = new DelegateCommand(StartSearch);
            ResetSearchCommand = new DelegateCommand(ResetSearchAction);
            SelectInitialCommand = new DelegateCommand<string>(LoadCustomersByInital);
            SelectCommand = new DelegateCommand<CustomerListEntry>(SelectCustomerAction);
            CreateOrderCommand = new DelegateCommand<CustomerListEntry>(CreateNewOrderForCustomerAction);
            EditOrderCommand = new DelegateCommand<OrderViewModel>(EditOrderAction);
            DeleteOrderCommand = new DelegateCommand<OrderViewModel>(DeleteOrderAction);
        }
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
        public ICommand StarSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        public ObservableCollection<OrderViewModel> AllOrders { get; } = new ObservableCollection<OrderViewModel>();
        public DelegateCommand<CustomerListEntry> SelectCommand { get; private set; }
        public DelegateCommand<CustomerListEntry> CreateOrderCommand { get; private set; }
        public DelegateCommand<OrderViewModel> EditOrderCommand { get; private set; }
        public DelegateCommand<OrderViewModel> DeleteOrderCommand { get; private set; }


        public ObservableCollection<CustomerListEntry> CustomersWithOrders { get; } = new ObservableCollection<CustomerListEntry>();
        public IOrderSearchCriteria Criteria { get { return _orderSearchCriteria; } }

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

        private CustomerListEntry _currentCustomer;
        /// <summary>
        /// Gets or sets the CurrentCustomer.
        /// </summary>
        /// <value>
        /// The CurrentCustomer.
        /// </value>
        public CustomerListEntry CurrentCustomer
        {
            get { return _currentCustomer; }
            set { SetProperty(ref _currentCustomer, value); }
        }

        #endregion
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
        #endregion
        #region Private methods

        private async void LoadCustomersByInital(string initial)
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            _customerSearchCriteria.Initial = initial != "#" ? initial : null;
            _customerSearchCriteria.ActiveCustomersOnly = true;
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
                var result = await _orderDataProvider.SearchOrders(_orderSearchCriteria, _pageSize, _page);
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
        private void AddOrdersToList(List<BestellungDto> orders)
        {
            foreach (BestellungDto bestellung in orders)
            {
                OrderViewModel vm = new OrderViewModel(bestellung, CurrentCustomer.Customer);
                AllOrders.Add(vm);
            }
        }
        private void ResetSearchAction()
        {
            AllOrders.Clear();
            CustomersWithOrders.ToList().ForEach(x => x.IsSelected = false);
            _orderSearchCriteria.Reset();
            _page = 0;
        }
        private void EditOrderAction(OrderViewModel order)
        {
            NavigationParameters pars = new NavigationParameters();
            pars.Add("orders", order);
            var moduleAWorkspace = new Uri("OrderEditView", UriKind.Relative);
            _regionManager.RequestNavigate("OrderDetailsRegion", moduleAWorkspace, pars);
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
            ResetSearchAction();
            CurrentCustomer = customer;
            _orderSearchCriteria.CustomerIds = new int[] { customer.ID };
            StartSearch();
        }
        private void CreateNewOrderForCustomerAction(CustomerListEntry customer)
        {
            if (customer != null)
            {
                BestellungDto order = new BestellungDto() { KundenId = customer.ID, Datum = DateTime.Now, StatusId = 1 };
                try
                {
                    order.BestellId = _orderDataProvider.AddOrder(order);
                    OrderViewModel vm = new OrderViewModel(order, customer.Customer);
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
        #endregion
    }
}
