using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArtikelViewModel : BindableBase
    {
        private ArtikelDto _artikel;
        public ArtikelViewModel(ArtikelDto artikel, Action<ArtikelViewModel> selectArtikel,
            Action<ArtikelViewModel> deleteArticle)
        {
            _artikel = artikel;
            SelectArtikel = new DelegateCommand<ArtikelViewModel>(selectArtikel);
            DeleteArtikel = new DelegateCommand<ArtikelViewModel>(deleteArticle);
        }

        /// <summary>
        /// Gets or sets the Markierungen.
        /// </summary>
        /// <value>
        /// The Markierungen.
        /// </value>
        public List<ArticleMarkingViewModel> Markierungen { get; } = new List<ArticleMarkingViewModel>();
      
        public string Artikelname { get { return _artikel.Name; } }
        public int? Lagerbestand { get { return _artikel.Lagerbestand; } }
        public decimal? Einzelpreis { get { return _artikel.Einzelpreis; } }
        public int ArtikelId { get { return _artikel.ArtikelId; } }
        public DelegateCommand<ArtikelViewModel> SelectArtikel { get; }
        public DelegateCommand<ArtikelViewModel> DeleteArtikel { get; }
    }
}
