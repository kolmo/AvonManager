using AvonManager.Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Windows.Input;

namespace AvonManager.Bestellungen.Controls
{
    public class OrderModulTaskButtonViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;
        public OrderModulTaskButtonViewModel()
        {

        }
        public OrderModulTaskButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            ShowOrderModule = new DelegateCommand(ShowOrderModuleAction);
        }
        public ICommand ShowOrderModule { get; set; }

        private void ShowOrderModuleAction()
        {
            var moduleAWorkspace = new Uri("OrderModuleWorkspace", UriKind.Relative);
            _regionManager.RequestNavigate("MainRegion", moduleAWorkspace, NavigationCompleted);
            _eventAggregator.GetEvent<ModuleChangedEvent>().Publish(new ModuleChangedEventArgs { ModuleTitle = "Bestellungsverwaltung" });
        }
        /// <summary>
        /// Callback method invoked when navigation has completed.
        /// </summary>
        /// <param name="result">Provides information about the result of the navigation.</param>
        private void NavigationCompleted(NavigationResult result)
        {
            // Exit if navigation was not successful
            if (result.Result != true) return;

            // Publish ViewRequestedEvent
            var navigationCompletedEvent = _eventAggregator.GetEvent<NavigationCompletedEvent>();
            navigationCompletedEvent.Publish("OrderModule");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
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
