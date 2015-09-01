using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace AvonManager.KundenHefte
{
    public class KundenHefteModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public KundenHefteModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.KundenHefteManagementView));
            _regionManager.RegisterViewWithRegion("KundenHefteRegion", typeof(Views.KundenSearchView));
            _regionManager.RegisterViewWithRegion("KundenHefteRegion", typeof(Views.KundenEditView));
            _regionManager.RegisterViewWithRegion("KundenHefteRegion", typeof(Views.HefteSearchView));
            _regionManager.RegisterViewWithRegion("KundenHefteRegion", typeof(Views.HefteEditView));
        }
    }
}
