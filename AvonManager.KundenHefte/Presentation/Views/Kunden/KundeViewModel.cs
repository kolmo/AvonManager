using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.KundenHefte.ViewModels
{
    public class KundeViewModel : BindableBase
    {
        public KundeViewModel() { }
        KundeDto _kunde;
        public KundeViewModel(KundeDto kunde, Action<KundeViewModel> editKunde, Action<KundeViewModel> deleteCustomerAction)
        {
            _kunde = kunde;
            EditKundeCommand = new DelegateCommand<KundeViewModel>(editKunde);
            DeleteCustomerCommand = new DelegateCommand<KundeViewModel>(deleteCustomerAction);
        }
        public int KundeId { get { return _kunde.KundenId; } }
        public string Vorname { get { return _kunde.Vorname; } }
        public string Nachname { get { return _kunde.Nachname; } }
        public string DisplayName { get { return $"{_kunde.Nachname}, {_kunde.Vorname}"; } }
        public bool? Inaktiv { get { return _kunde.Inaktiv; } }
        public bool? GetsBrochure { get { return _kunde.BekommtHeft; } }
        public DelegateCommand<KundeViewModel> EditKundeCommand { get; private set; }
        public DelegateCommand<KundeViewModel> DeleteCustomerCommand { get; private set; }
    }
}
