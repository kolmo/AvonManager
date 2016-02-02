using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace AvonManager.KundenHefte
{
    public class BrochureModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        public BrochureModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion,
               typeof(Controls.BrochureModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.BrochureButtonRegion, typeof(Controls.BrochureModuleTaskButton));
            _container.RegisterType<object, Presentation.Views.BrochureManagementView>("BrochureModuleWorkspace");
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.BrochureSearchRegion, typeof(Views.HefteSearchView));
            _container.RegisterType<object, Views.HeftEditView>("HeftEditView");
        }
    }
}
