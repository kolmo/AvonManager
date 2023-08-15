using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using System;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class MarkingListEntryViewModel : ListEntryBaseViewModel<MarkingListEntryViewModel>
    {
        private MarkierungDto _markierung;

        public MarkingListEntryViewModel(MarkierungDto markierung)
        {
            _markierung = markierung;
        }

        public MarkingListEntryViewModel(MarkierungDto markierung, Action<MarkingListEntryViewModel> editAction, Action<MarkingListEntryViewModel> deleteAction)
            : base(editAction, deleteAction)
        {
            _markierung = markierung;
        }

        /// <summary>
        /// Gets or sets the Titel.
        /// </summary>
        /// <value>
        /// The Titel.
        /// </value>
        public string Titel
        {
            get { return _markierung.Titel; }
        }

        /// <summary>
        /// Gets or sets the Bemerkung.
        /// </summary>
        /// <value>
        /// The Bemerkung.
        /// </value>
        public string Bemerkung
        {
            get { return _markierung.Bemerkung; }
        }

        /// <summary>
        /// Gets or sets the FarbeARGB.
        /// </summary>
        /// <value>
        /// The FarbeARGB.
        /// </value>
        public int? FarbeARGB
        {
            get { return _markierung.FarbeARGB; }
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

        public int MarkierungId
        {
            get { return _markierung.MarkierungId; }
        }
    }
}