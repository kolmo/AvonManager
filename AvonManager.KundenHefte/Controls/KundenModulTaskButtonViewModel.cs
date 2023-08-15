using AvonManager.Common.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace AvonManager.KundenHefte.ViewModels
{
    public class KundenModulTaskButtonViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;
        public KundenModulTaskButtonViewModel()
        {

        }
        public KundenModulTaskButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            ShowArtikelModule = new DelegateCommand(ShowArtikelModuleAction);
        }
        public ICommand ShowArtikelModule { get; set; }

        private void ShowArtikelModuleAction()
        {
            var moduleAWorkspace = new Uri("KundenModuleWorkspace", UriKind.Relative);
            _regionManager.RequestNavigate("MainRegion", moduleAWorkspace, NavigationCompleted);
            _eventAggregator.GetEvent<ModuleChangedEvent>().Publish(new ModuleChangedEventArgs { ModuleTitle = "Kundenverwaltung" });
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
            navigationCompletedEvent.Publish("KundenModule");
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
