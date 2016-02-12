using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.Common.Helpers;
using AvonManager.Common.Base;
using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.Bestellungen.Presentation.Views
{
    public class OrderEditViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private struct Backingfields
        {
            public DateTime? Datum;
            public string Bemerkung;
            public int? StatusId;
            public bool? Loeschvormerkung;
        }
        private const string LOAD = "LOAD";
        private Backingfields bFields, clone;
        private readonly IOrderDataProvider _orderDataProvider;
        private bool _isInitializing = false;
        private BestellungDto _currentOrder;
        private KundeDto _orderCustomer;
        private readonly IKundenDataProvider _customerDataProvider;
        public OrderEditViewModel(IOrderDataProvider orderDataProvider,
            IKundenDataProvider customerDataProvider,
            IEventAggregator eventAggregator
            , BusyFlagsManager busyFlagsManager) : base(busyFlagsManager, eventAggregator)
        {
            _orderDataProvider = orderDataProvider;
            _customerDataProvider = customerDataProvider;
            AddDetail = new DelegateCommand(AddDetailAction, () => { return !IsOrderReadOnly; });
            RemoveDetail = new DelegateCommand<object>(RemoveDetailAction, x => { return !IsOrderReadOnly; });
        }

        #region Properties
        #region Commands
        public DelegateCommand AddDetail { get; }
        public DelegateCommand<object> RemoveDetail { get; }
        #endregion
        public ObservableCollection<OrderDetailsViewModel> OrderDetails { get; } = new ObservableCollection<OrderDetailsViewModel>();
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();

        public string CustomerName { get { return $"{_orderCustomer?.Vorname} {_orderCustomer?.Nachname}"; } }


        private List<BestellstatusDto> _statusList;
        /// <summary>
        /// Gets or sets the StatusList.
        /// </summary>
        /// <value>
        /// The StatusList.
        /// </value>
        public List<BestellstatusDto> StatusList
        {
            get { return _statusList; }
            set { SetProperty(ref _statusList, value); }
        }

        /// <summary>
        /// Gets or sets the Bemerkung.
        /// </summary>
        /// <value>
        /// The Bemerkung.
        /// </value>
        public string Bemerkung
        {
            get { return bFields.Bemerkung; }
            set { SetProperty(ref bFields.Bemerkung, value); }
        }

        /// <summary>
        /// Gets or sets the Loeschvormerkung.
        /// </summary>
        /// <value>
        /// The Loeschvormerkung.
        /// </value>
        public bool? Loeschvormerkung
        {
            get { return bFields.Loeschvormerkung; }
            set { SetProperty(ref bFields.Loeschvormerkung, value); }
        }

        /// <summary>
        /// Gets or sets the Datum.
        /// </summary>
        /// <value>
        /// The Datum.
        /// </value>
        public DateTime? Datum
        {
            get { return bFields.Datum; }
            set { SetProperty(ref bFields.Datum, value); }
        }

        /// <summary>
        /// Gets or sets the StatusId.
        /// </summary>
        /// <value>
        /// The StatusId.
        /// </value>
        public int? StatusId
        {
            get { return bFields.StatusId; }
            set
            {
                SetProperty(ref bFields.StatusId, value);
                IsOrderReadOnly = bFields.StatusId == Constants.OrderStates.ClosedState;
            }
        }

        public decimal Bestellwert
        {
            get
            {
                decimal wert = 0;
                if (this.OrderDetails != null)
                {
                    foreach (var detail in OrderDetails)
                    {
                        if (detail.Menge.HasValue && detail.Einzelpreis.HasValue)
                        {
                            wert += detail.Menge.Value * detail.Einzelpreis.Value;
                        }
                    }
                }
                return wert;
            }
        }

        private bool _isOrderReadOnly;
        /// <summary>
        /// Gets or sets the IsOrderReadOnly.
        /// </summary>
        /// <value>
        /// The IsOrderReadOnly.
        /// </value>
        public bool IsOrderReadOnly
        {
            get { return _isOrderReadOnly; }
            set
            {
                SetProperty(ref _isOrderReadOnly, value);
                AddDetail.RaiseCanExecuteChanged();
                RemoveDetail.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
            {
                SaveOrder();
            }
            return ok;
        }

        #endregion

        #region Public methods
        public async void LoadSupplementData()
        {
            _isInitializing = true;
            try
            {
                StatusList = await _orderDataProvider.ListAllOrderStatusValues();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            _isInitializing = false;
        }
        #endregion
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            BusyFlagsMgr.ResetAllBusyFlags();
            var orderParameter = navigationContext.Parameters.FirstOrDefault();
            if (orderParameter.Value is OrderViewModel)
            {
                int orderId = ((OrderViewModel)orderParameter.Value).OrderId;
                LoadCurrentOrder(orderId);
            }
            else
            {
                _currentOrder = new BestellungDto();
                InitProperties();
            }
        }

        #region Private helper methods
        private async void LoadCurrentOrder(int orderId)
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                _currentOrder = await _orderDataProvider.LoadOrder(orderId);
                var custList = await _customerDataProvider.LoadCustomers(new int[] { _currentOrder.KundenId.Value });
                _orderCustomer = custList.FirstOrDefault();
                InitProperties();
                OrderDetails.Clear();
                var orderDetails = await _orderDataProvider.ListOrderDetailsByOrder(_currentOrder.BestellId);
                foreach (var orderDetail in orderDetails)
                {
                    OrderDetailsViewModel vm = new OrderDetailsViewModel(orderDetail, _orderDataProvider);
                    vm.PropertyChanged += Vm_PropertyChanged;
                    OrderDetails.Add(vm);
                }
                OnPropertyChanged(nameof(Bestellwert));
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
        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Menge") || e.PropertyName.Equals("Einzelpreis"))
            {
                OnPropertyChanged(nameof(Bestellwert));
            }
        }
        private void InitProperties()
        {
            if (_currentOrder != null)
            {
                _isInitializing = true;
                OnPropertyChanged(nameof(CustomerName));
                Datum = _currentOrder.Datum;
                Bemerkung = _currentOrder.Bemerkung;
                Loeschvormerkung = _currentOrder.Loeschvormerkung;
                StatusId = _currentOrder.StatusId;
                IsOrderReadOnly = StatusId == Constants.OrderStates.ClosedState;
                clone = bFields;
                OrderDetails.Clear();
                _isInitializing = false;
            }
        }
        private void SaveOrder()
        {
            _currentOrder.Bemerkung = Bemerkung;
            _currentOrder.StatusId = StatusId;
            _currentOrder.Loeschvormerkung = Loeschvormerkung;
            try
            {
                _orderDataProvider.UpdateOrder(_currentOrder);
                clone = bFields;
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void AddDetailAction()
        {
            if (_currentOrder != null)
            {
                BestelldetailDto detail = new BestelldetailDto
                {
                    BestellId = _currentOrder.BestellId
                };
                PrefillDetails(detail);
                try
                {
                    detail.DetailId = _orderDataProvider.AddOrderDetail(detail);
                    OrderDetailsViewModel vm = new OrderDetailsViewModel(detail, _orderDataProvider);
                    vm.PropertyChanged += Vm_PropertyChanged;
                    OrderDetails.Add(vm);
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void RemoveDetailAction(object obj)
        {
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation()
            {
                Title = "Nachfrage",
                Content = "Soll das Detail gelöscht werden?",
                Entity = obj
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteDetailFromDb);
        }
        private void DeleteDetailFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation.Confirmed && _currentOrder != null)
            {
                OrderDetailsViewModel orderDetailToRemove = confirmation.Entity as OrderDetailsViewModel;
                try
                {
                    _orderDataProvider.DeleteOrderDetail(orderDetailToRemove.OrderDetailId);
                    OrderDetails.Remove(orderDetailToRemove);
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void PrefillDetails(BestelldetailDto detail)
        {
            if (OrderDetails.Any())
            {
                OrderDetailsViewModel last = OrderDetails.Last();
                detail.Jahr = last.Jahr;
                detail.Campagne = last.Campagne;
            }
            else
            {
                detail.Jahr = DateTime.Now.Year;
                detail.Menge = 1;
            }
        }
        #endregion
    }
}
