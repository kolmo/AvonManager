using AvonManager.Common.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Linq;
namespace AvonManager.ArtikelModule
{
    public class ArtikelModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer _container;
        public ArtikelModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.TaskButtonRegion, typeof(Controls.ArtikelModuleTaskButton));
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.ArticleButtonRegion, typeof(Controls.ArtikelModuleTaskButton));
            _container.RegisterType<object, Views.ArtikelManagementView>("ArtikelModuleWorkspace");
            _container.RegisterType<object, NoSelectionPlaceHolderView>("NoSelectionPlaceHolder");
            regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.ArticleSearchRegion, typeof(Views.ArtikelSearchView));
            _container.RegisterType<object, Views.ArtikelDetailsView>("ArtikelDetailsWorkspace");
        }
    }
}
