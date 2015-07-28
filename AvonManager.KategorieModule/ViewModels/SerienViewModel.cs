using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.KategorieModule.ViewModels
{
    public class SerienViewModel : BindableBase
    {
        private readonly IUnityContainer _container;
        public SerienViewModel(IUnityContainer container)
        {
            _container = container;
        }
    }
}
