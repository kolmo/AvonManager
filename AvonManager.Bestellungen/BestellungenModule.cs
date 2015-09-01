using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
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
        public BestellungenModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.BestellungManagementView));
            _regionManager.RegisterViewWithRegion("BestellungRegion", typeof(Views.BestellungSearchView));
            _regionManager.RegisterViewWithRegion("BestellungRegion", typeof(Views.BestellungEditView));
        }
    }
}
