using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArtikelViewModel : BindableBase
    {
        private Artikel _artikel;
        public ArtikelViewModel(Artikel artikel)
        {
            _artikel = artikel;
        }

        private List<MarkierungDto> _markierungen;
        /// <summary>
        /// Gets or sets the Markierungen.
        /// </summary>
        /// <value>
        /// The Markierungen.
        /// </value>
        public List<MarkierungDto> Markierungen
        {
            get { return _markierungen; }
            set
            {
                if (_markierungen != value)
                {
                    _markierungen = value;
                    OnPropertyChanged(() => this.Markierungen);
                }
            }
        }
        public string Artikelname { get { return _artikel.Name; } }
        public int? Lagerbestand { get { return _artikel.Lagerbestand; } }
        public decimal? Einzelpreis { get { return _artikel.Einzelpreis; } }
        public int ArtikelId { get { return _artikel.ArtikelId; } }
    }
}
