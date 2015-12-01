using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Bestellungen
{
    public class BestellungenModule : IModule
    {
        private readonly IRegionManager _regionManager;
       IUnityContainer _container;
        public BestellungenModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<object, Views.BestellungManagementView>("OrderModuleWorkspace");
            _container.RegisterType<object, Views.OrderEditView>("OrderEditView");
            _regionManager.RegisterViewWithRegion("TaskButtonRegion", typeof(Controls.OrderModuleTaskButton));
            _regionManager.RegisterViewWithRegion(AvonManager.Common.RegionNames.OrderButtonRegion, typeof(Controls.OrderModuleTaskButton));
            _regionManager.RegisterViewWithRegion("OrderSearchRegion", typeof(Views.OrderSearchView));
        }
    }
}
