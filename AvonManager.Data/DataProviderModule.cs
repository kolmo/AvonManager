using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Data
{
    public class DataProviderModule : IModule
    {
        IUnityContainer _container;
        public DataProviderModule(IUnityContainer container)
        {
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<Interfaces.IObjectMapper, Helpers.EntityBOMapper>();
            _container.RegisterType<IArtikelDataProvider,ArtikelDataProvider>();
            _container.RegisterType<IMarkierungenDataProvider,MarkierungenDataProvider>();
            _container.RegisterType<IKategorieProvider,KategorieDataProvider>();
        }
    }
}
