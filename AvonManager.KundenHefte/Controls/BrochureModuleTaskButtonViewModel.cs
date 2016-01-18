using AvonManager.Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Windows.Input;

namespace AvonManager.KundenHefte.Controls
{
    public class BrochureModuleTaskButtonViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;
        public BrochureModuleTaskButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            ShowBrochureModule = new DelegateCommand(ShowBrochureModuleAction);
        }
        public ICommand ShowBrochureModule { get; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
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
            navigationCompletedEvent.Publish("BrochureModule");
        }
        private void ShowBrochureModuleAction()
        {
            var moduleAWorkspace = new Uri("BrochureModuleWorkspace", UriKind.Relative);
            _regionManager.RequestNavigate("MainRegion", moduleAWorkspace, NavigationCompleted);
            _eventAggregator.GetEvent<ModuleChangedEvent>().Publish(new ModuleChangedEventArgs { ModuleTitle = "Hefteverwaltung" });
        }

    }
}
