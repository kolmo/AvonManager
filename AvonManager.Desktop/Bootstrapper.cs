using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using System;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.Desktop
{
    class Bootstrapper : UnityBootstrapper
    {
      
        protected override DependencyObject CreateShell()
        {
            return new Shell();
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
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(TabControl), Container.TryResolve<TabControlAdapter>());

            return mappings;
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //ViewModelLocationProvider.SetDefaultViewModelFactory(viewModelType => this.Container.Resolve(viewModelType));
        }
    }

}
