using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;

namespace AvonManager.ArtikelModule.Views
{
    public class SeriesListEntryViewModel : ListEntryBaseViewModel<SeriesListEntryViewModel>
    {
        private SerieDto _serie;
        public SeriesListEntryViewModel(SerieDto serie)
        {
            _serie = serie;
        }
        public SeriesListEntryViewModel(SerieDto serie, Action<SeriesListEntryViewModel> editAction, Action<SeriesListEntryViewModel> deleteAction)
            : base(editAction, deleteAction)
        {
            _serie = serie;
        }
        public string Name
        {
            get { return _serie != null ? _serie.Name : "<Keine Serie>"; }
            set
            {
                if (_serie != null)
                {
                    _serie.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public int? SerienId { get { return _serie?.SerienId; } }
        public override string ToString()
        {
            return this.Name;
        }
        private int _articleCount;
        /// <summary>
        /// Gets or sets the ArticleCount.
        /// </summary>
        /// <value>
        /// The ArticleCount.
        /// </value>
        public int ArticleCount
        {
            get { return _articleCount; }
            set { SetProperty(ref _articleCount, value); }
        }
    }
}
