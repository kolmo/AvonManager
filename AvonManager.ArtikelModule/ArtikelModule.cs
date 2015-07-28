using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

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
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.ArtikelPage));
        }
    }
}
