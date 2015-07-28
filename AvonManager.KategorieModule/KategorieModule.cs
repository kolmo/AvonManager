using AvonManager.Interfaces;
using AvonManager.KategorieModule.ViewModels;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.KategorieModule
{
    public class KategorieModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IKategorieProvider _kategorieProvider;
        private readonly IUnityContainer _container;
        public KategorieModule(IRegionManager regionManager, IKategorieProvider kategorieProvider, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
            _kategorieProvider = kategorieProvider;
        }
        public void Initialize()
        {
            ViewModelLocationProvider.Register(typeof(Views.KategorieView).FullName, () => { return new KategorienViewModel(_kategorieProvider); });
            ViewModelLocationProvider.Register(typeof(Controls.SerienControl).FullName, () => { return new SerienViewModel(_container); });
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.KategorieView));
        }
    }
}
