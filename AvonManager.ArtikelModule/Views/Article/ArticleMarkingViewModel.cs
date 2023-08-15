using System;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Prism.Mvvm;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArticleMarkingViewModel : BindableBase
    {
        private ArtikelDto _article;
        private MarkierungDto _markierung;
        IMarkierungenDataProvider _markierungenDataProvider;
        ArtikelMarkierungenDto _articleMarking;
        public ArticleMarkingViewModel() { }
        public ArticleMarkingViewModel(ArtikelDto article, MarkierungDto markierung, ArtikelMarkierungenDto articleMarking, IMarkierungenDataProvider markierungenDataProvider, bool isAssigned)
        {
            _article = article;
            _markierung = markierung;
            _articleMarking = articleMarking;
            _isAssigned = isAssigned;
            _markierungenDataProvider = markierungenDataProvider;
        }


        private bool _isAssigned;
        /// <summary>
        /// Gets or sets the IsAssigned.
        /// </summary>
        /// <value>
        /// The IsAssigned.
        /// </value>
        public bool IsAssigned
        {
            get { return _isAssigned; }
            set
            {
                SetProperty(ref _isAssigned, value);
                AddOrDeleteAssignment();
            }
        }

        public int? ColorCode
        {
            get
            {
                return _markierung.FarbeARGB;
            }
        }

        public string MarkingTitle
        {
            get
            {
                return _markierung.Titel;
            }
        }

        public int ID
        {
            get
            {
                return _markierung.MarkierungId;
            }
        }

        #region Private Methods
        private void AddOrDeleteAssignment()
        {
            if (IsAssigned)
            {
                _markierungenDataProvider.AddMarkierungArtikel(_articleMarking);
            }
            else
            {
                _markierungenDataProvider.DeleteMarkierungArtikel(_articleMarking);
            }
        }

        #endregion
    }
}
