using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace AvonManager.KundenHefte
{
    public class CustomerModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        public CustomerModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion, typeof(Controls.KundenModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CustomerButtonRegion, typeof(Controls.KundenModuleTaskButton));
            _container.RegisterType<object, Views.KundenHefteManagementView>("KundenModuleWorkspace");
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CustomerSearchRegion, typeof(Views.KundenSearchView));
            _container.RegisterType<object, Views.KundenEditView>("KundenEditView");
        }
    }
}
