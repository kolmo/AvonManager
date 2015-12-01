using AvonManager.Desktop;
using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace AvonManager
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            IRegionManager regionManager = bootstrapper.Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Common.RegionNames.MainRegion, typeof(MainView));
            bootstrapper.Container.RegisterType<object, MainView>("HomeView");
        }
       
    }
}
