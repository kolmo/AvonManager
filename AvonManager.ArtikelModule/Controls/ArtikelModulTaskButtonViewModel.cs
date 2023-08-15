﻿using AvonManager.Common;
using AvonManager.Common.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArtikelModulTaskButtonViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;
        public ArtikelModulTaskButtonViewModel()
        {

        }
        public ArtikelModulTaskButtonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            ShowArtikelModule = new DelegateCommand(ShowArtikelModuleAction);
        }
        public ICommand ShowArtikelModule { get; set; }

        private void ShowArtikelModuleAction()
        {
            var moduleAWorkspace = new Uri("ArtikelModuleWorkspace", UriKind.Relative);
            _regionManager.RequestNavigate(RegionNames.MainRegion, moduleAWorkspace, NavigationCompleted);
            _eventAggregator.GetEvent<ModuleChangedEvent>().Publish(new ModuleChangedEventArgs {ModuleTitle = "Artikel-Verwaltung" });

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
            IRegion artikelRegion = _regionManager.Regions[RegionNames.ArticleSearchRegion];
            artikelRegion?.Activate(artikelRegion.Views.FirstOrDefault());

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
