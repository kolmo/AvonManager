using AvonManager.Interfaces;
using AvonManager.Interfaces.Criteria;
using Prism.Ioc;
using Prism.Modularity;
using Unity;

namespace AvonManager.Data
{
    public class DataProviderModule : IModule
    {
       
   

        public void OnInitialized(IContainerProvider containerProvider)
        {
           ;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Interfaces.IObjectMapper, Helpers.EntityBOMapper>();
            containerRegistry.Register<IArtikelDataProvider, ArtikelDataProvider>();
            containerRegistry.Register<IMarkierungenDataProvider, MarkierungenDataProvider>();
            containerRegistry.Register<IKategorieProvider, KategorieDataProvider>();
            containerRegistry.Register<ISerienDataProvider, SerienDataProvider>();
            containerRegistry.Register<IHefteDataProvider, HefteDataProvider>();
            containerRegistry.Register<IKundenDataProvider, KundenDataProvider>();
            containerRegistry.Register<IOrderDataProvider, OrderDataProvider>();
            containerRegistry.Register<ICustomerSearchCriteria, CustomerSearchCriteria>();
            containerRegistry.Register<IOrderSearchCriteria, OrderSearchCriteria>();
            containerRegistry.Register<IBrochureSearchCriteria, SearchCriteria.BrochureSearchCriteria>();
            containerRegistry.Register<IArticleSearchCriteria, SearchCriteria.ArticleSearchCriteria>();
        }
    }
}
