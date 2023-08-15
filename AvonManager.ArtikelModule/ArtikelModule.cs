using AvonManager.Common.Controls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AvonManager.ArtikelModule
{
    public class ArtikelModule : IModule
    {
        private readonly IRegionManager regionManager;

        public ArtikelModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion, typeof(Controls.ArtikelModuleTaskButton));
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.ArticleButtonRegion, typeof(Controls.ArtikelModuleTaskButton));
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.ArticleSearchRegion, typeof(Views.ArtikelSearchView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Views.ArtikelManagementView>("ArtikelModuleWorkspace");
            containerRegistry.Register<object, NoSelectionPlaceHolderView>("NoSelectionPlaceHolder");
            containerRegistry.Register<object, Views.ArtikelDetailsView>("ArtikelDetailsWorkspace");
        }
    }
}