using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using System;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class CategoryListEntryViewModel : ListEntryBaseViewModel<CategoryListEntryViewModel>
    {
        private KategorieDto _kategorie;
        public CategoryListEntryViewModel() { }
        public CategoryListEntryViewModel(KategorieDto kategorie)
        {
            _kategorie = kategorie;
        }
        public CategoryListEntryViewModel(KategorieDto kategorie, Action<CategoryListEntryViewModel> editCategory, Action<CategoryListEntryViewModel> deleteCategory)
            :base(editCategory, deleteCategory)
        {
            _kategorie = kategorie;
        }

        #region Properties
        public int KategorieId { get { return _kategorie.KategorieId; } }
        public KategorieDto Kategorie { get { return _kategorie; } }
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Kategoriename
        {
            get { return _kategorie.Name; }
        }
        /// <summary>
        /// Gets or sets the Bemerkung.
        /// </summary>
        /// <value>
        /// The Bemerkung.
        /// </value>
        public string Bemerkung
        {
            get { return _kategorie.Bemerkung; }
        }
        /// <summary>
        /// Gets or sets the Logo.
        /// </summary>
        /// <value>
        /// The Logo.
        /// </value>
        public byte[] Logo
        {
            get { return _kategorie.Logo; }
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

        #endregion

    }
}
