using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AvonManager.KundenHefte
{
    public class CustomerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CustomerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion,
                typeof(Controls.KundenModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CustomerButtonRegion, typeof(Controls.KundenModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CustomerSearchRegion, typeof(Views.KundenSearchView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Views.KundenHefteManagementView>("KundenModuleWorkspace");
            containerRegistry.Register<object, Views.KundenEditView>("KundenEditView");
        }
    }
}