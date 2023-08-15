using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AvonManager.Bestellungen
{
    public class BestellungenModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public BestellungenModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion,
                typeof(Controls.OrderModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.OrderButtonRegion, typeof(Controls.OrderModuleTaskButton));
            _regionManager.RegisterViewWithRegion("OrderSearchRegion", typeof(Views.OrderSearchView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Views.BestellungManagementView>("OrderModuleWorkspace");
            containerRegistry.Register<object, Views.OrderEditView>("OrderEditView");
        }
    }
}