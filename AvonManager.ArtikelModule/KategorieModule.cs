using AvonManager.ArtikelModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AvonManager.ArtikelModule
{
    public class KategorieModule : IModule
    {
        private readonly IRegionManager regionManager;

        public KategorieModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion,
                 typeof(Controls.KategorieModuleTaskButton));
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CategoryButtonRegion, typeof(Controls.KategorieModuleTaskButton));
            regionManager.RegisterViewWithRegion("CategorySearchRegion", typeof(CategorySearchView));
            regionManager.RegisterViewWithRegion("SeriesSearchRegion", typeof(SeriesSearchView));
            regionManager.RegisterViewWithRegion("MarkingSearchRegion", typeof(MarkingSearchView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Views.ArticleClassificationPage>("ArticleClassificationModuleWorkspace");
        }
    }
}