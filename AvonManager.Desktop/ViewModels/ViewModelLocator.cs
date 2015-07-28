/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:AvonManager.ViewModels"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using Microsoft.Practices.ServiceLocation;
using AvonManager.Data;
using AvonManager.Model;

namespace AvonManager.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                // SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<ClipboardManager>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ArtikelViewModel>();
            SimpleIoc.Default.Register<BestellungViewModel>();
            SimpleIoc.Default.Register<KundenViewModel>();
            SimpleIoc.Default.Register<SerienViewModel>();
            SimpleIoc.Default.Register<MarkierungenViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public ArtikelViewModel ArtikelVM
        {
            get
            {
                //return new ArtikelViewModel();
                return ServiceLocator.Current.GetInstance<ArtikelViewModel>();
            }
        }
        public BestellungViewModel BestellungVM { get { return ServiceLocator.Current.GetInstance<BestellungViewModel>(); } }
        public KundenViewModel KundenVM { get { return ServiceLocator.Current.GetInstance<KundenViewModel>(); } }
        public SerienViewModel SerienVM { get { return ServiceLocator.Current.GetInstance<SerienViewModel>(); } }
        public MarkierungenViewModel MarkierungenVM { get { return ServiceLocator.Current.GetInstance<MarkierungenViewModel>(); } }
    }
}