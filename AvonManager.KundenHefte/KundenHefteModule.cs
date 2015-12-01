using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace AvonManager.KundenHefte
{
    public class KundenHefteModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        public KundenHefteModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("TaskButtonRegion", typeof(Controls.KundenModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CustomerButtonRegion, typeof(Controls.KundenModuleTaskButton));
            _container.RegisterType<object, Views.KundenHefteManagementView>("KundenModuleWorkspace");
            _regionManager.RegisterViewWithRegion("KundenSearchRegion", typeof(Views.KundenSearchView));
            _regionManager.RegisterViewWithRegion("HefteSearchRegion", typeof(Views.HefteSearchView));
            _container.RegisterType<object, Views.HeftEditView>("HeftEditView");
            _container.RegisterType<object, Views.KundenEditView>("KundenEditView");
        }
    }
}
