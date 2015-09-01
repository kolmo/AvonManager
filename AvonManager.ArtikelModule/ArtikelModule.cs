using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.Linq;
namespace AvonManager.ArtikelModule
{
    public class ArtikelModule : IModule
    {
        private readonly IRegionManager regionManager;
        public ArtikelModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.ArtikelManagementView));
            IRegion mainRegion = regionManager.Regions["MainRegion"];
            mainRegion.Activate(mainRegion.Views.FirstOrDefault());
            regionManager.RegisterViewWithRegion("ArtikelRegion", typeof(Views.ArtikelSearchPage));
            regionManager.RegisterViewWithRegion("ArtikelRegion", typeof(Views.KategoriePage));
        }
    }
}
