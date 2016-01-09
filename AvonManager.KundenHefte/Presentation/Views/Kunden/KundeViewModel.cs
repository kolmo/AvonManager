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
        public KundeViewModel(KundeDto kunde)
        {
            _kunde = kunde;
        }
        public int KundeId { get { return _kunde.KundenId; } }
        public string Vorname { get { return _kunde.Vorname; } }
        public string Nachname { get { return _kunde.Nachname; } }
        public string DisplayName { get { return $"{_kunde.Nachname}, {_kunde.Vorname}"; } }
        public bool? Inaktiv { get { return _kunde.Inaktiv; } }
        public bool? GetsBrochure { get { return _kunde.BekommtHeft; } }
        private int _orderCount;
     
        public int OrderCount
        {
            get { return _orderCount; }
            set { SetProperty(ref _orderCount, value); }
        }
    }
}
