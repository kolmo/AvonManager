using AvonManager.Interfaces;
using AvonManager.Interfaces.Criteria;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

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
            _container.RegisterType<ISerienDataProvider, SerienDataProvider>();
            _container.RegisterType<IHefteDataProvider, HefteDataProvider>();
            _container.RegisterType<IKundenDataProvider, KundenDataProvider>();
            _container.RegisterType<IOrderDataProvider, OrderDataProvider>();
            _container.RegisterType<ICustomerSearchCriteria, CustomerSearchCriteria>();
            _container.RegisterType<IOrderSearchCriteria, OrderSearchCriteria>();
            _container.RegisterType<IBrochureSearchCriteria, SearchCriteria.BrochureSearchCriteria>();
            _container.RegisterType<IArticleSearchCriteria, SearchCriteria.ArticleSearchCriteria>();
        }
    }
}
