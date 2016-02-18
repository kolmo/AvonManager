using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using System.Windows;

namespace AvonManager.Desktop
{
    class Bootstrapper : UnityBootstrapper
    {
      
        protected override DependencyObject CreateShell()
        {
            return new Shell(Container);
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(new Uri("/AvonManager.Desktop;component/ModulesCatalog.xaml", UriKind.Relative));
        }
    }

}
