using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArticleViewModel : BindableBase
    {
        private ArtikelDto _artikel;
        public ArticleViewModel(ArtikelDto artikel)
        {
            _artikel = artikel;
        }
        public ArtikelDto Dto { get { return _artikel; } }
        /// <summary>
        /// Gets or sets the Markierungen.
        /// </summary>
        /// <value>
        /// The Markierungen.
        /// </value>
        public ObservableCollection<ArticleMarkingViewModel> Markierungen { get; } = new ObservableCollection<ArticleMarkingViewModel>();
      
        public string Artikelname { get { return _artikel.Name; } }

        private string _serie;
        /// <summary>
        /// Gets or sets the Serie.
        /// </summary>
        /// <value>
        /// The Serie.
        /// </value>
        public string Serie
        {
            get { return _serie; }
            set { SetProperty(ref _serie, value); }
        }

        public int? Lagerbestand { get { return _artikel.Lagerbestand; } }
        public decimal? Einzelpreis { get { return _artikel.Einzelpreis; } }
        public int ArtikelId { get { return _artikel.ArtikelId; } }
    }
}
