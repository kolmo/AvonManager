using AvonManager.Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class KategorieModulTaskButtonViewModel
    {
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;
        public KategorieModulTaskButtonViewModel()
        {

        }
        public KategorieModulTaskButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            ShowKategorieModule = new DelegateCommand(ShowKategorieModuleAction);
        }
        public ICommand ShowKategorieModule { get; set; }

        private void ShowKategorieModuleAction()
        {
            var moduleAWorkspace = new Uri("ArticleClassificationModuleWorkspace", UriKind.Relative);
            _regionManager.RequestNavigate("MainRegion", moduleAWorkspace, NavigationCompleted);

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
            navigationCompletedEvent.Publish("ModuleA");
        }

    }
}
