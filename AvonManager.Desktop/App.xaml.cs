using AvonManager.Desktop;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Windows;

namespace AvonManager
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => Container.Resolve<Shell>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Common.Controls.NoSelectionPlaceHolderView>("NoSelectionPlaceHolderView");
            containerRegistry.Register<object, MainView>("HomeView");
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return ModuleCatalog.CreateFromXaml(new Uri("/AvonManager.Desktop;component/ModulesCatalog.xaml", UriKind.Relative));
        }
    }
}