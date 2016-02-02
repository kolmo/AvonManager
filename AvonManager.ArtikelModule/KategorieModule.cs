using AvonManager.ArtikelModule.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace AvonManager.ArtikelModule
{
    public class KategorieModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer _container;
        public KategorieModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion,
                 typeof(Controls.KategorieModuleTaskButton));
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.CategoryButtonRegion, typeof(Controls.KategorieModuleTaskButton));
            regionManager.RegisterViewWithRegion("CategorySearchRegion", typeof(CategorySearchView));
            regionManager.RegisterViewWithRegion("SeriesSearchRegion", typeof(SeriesSearchView));
            regionManager.RegisterViewWithRegion("MarkingSearchRegion", typeof(MarkingSearchView));
            _container.RegisterType<object, Views.ArticleClassificationPage>("ArticleClassificationModuleWorkspace");
        }
    }
}
