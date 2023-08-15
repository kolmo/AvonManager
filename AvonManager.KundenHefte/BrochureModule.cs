
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace AvonManager.KundenHefte
{
    public class BrochureModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public BrochureModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
      
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion,
               typeof(Controls.BrochureModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.BrochureButtonRegion, typeof(Controls.BrochureModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.BrochureSearchRegion, typeof(Views.HefteSearchView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Presentation.Views.BrochureManagementView>("BrochureModuleWorkspace");
            containerRegistry.Register<object, Views.HeftEditView>("HeftEditView");
        }
    }
}
