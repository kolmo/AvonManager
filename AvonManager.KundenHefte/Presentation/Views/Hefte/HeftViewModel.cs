using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;

namespace AvonManager.KundenHefte.ViewModels
{
    public class HeftViewModel : BindableBase
    {
        public HeftViewModel()
        {

        }
        HeftDto _heft;
        public HeftViewModel(HeftDto heft, Action<HeftViewModel> editHeft, Action<HeftViewModel> deleteBrochure)
        {
            _heft = heft;
            EditHeftCommand = new DelegateCommand<HeftViewModel>(editHeft);
            DeleteBrochureCommand = new DelegateCommand<HeftViewModel>(deleteBrochure);
        }
        public int HeftId { get { return _heft.HeftId; } }
        public string Titel { get { return _heft.Titel; }  }
        public int? Jahr { get { return _heft.Jahr; }  }
        public string Beschreibung { get { return _heft.Beschreibung; }  }
        public byte[] Bild { get { return _heft.Bild; }  }
        public DelegateCommand<HeftViewModel> EditHeftCommand { get; private set; }
        public DelegateCommand<HeftViewModel> DeleteBrochureCommand { get; private set; }
    }
}
