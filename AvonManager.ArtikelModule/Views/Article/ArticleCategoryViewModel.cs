using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Prism.Mvvm;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class ArticleCategoryViewModel : BindableBase
    {
        private ArtikelDto _article;
        private KategorieDto _category;
        IKategorieProvider _categoryDataProvider;
        ArticleCategoryDto _articleCategory;
        public ArticleCategoryViewModel() { }
        public ArticleCategoryViewModel(ArtikelDto article, KategorieDto category, ArticleCategoryDto articleCategory, IKategorieProvider categoryDataProvider, bool isAssigned)
        {
            _article = article;
            _category = category;
            _articleCategory = articleCategory;
            _isAssigned = isAssigned;
            _categoryDataProvider = categoryDataProvider;
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

        public string CategoryTitle
        {
            get
            {
                return _category.Name;
            }
        }

        public int ID
        {
            get
            {
                return _category.KategorieId;
            }
        }

        #region Private Methods
        private void AddOrDeleteAssignment()
        {
            if (IsAssigned)
            {
                _categoryDataProvider.AddCategoryArtikel(_articleCategory);
            }
            else
            {
                _categoryDataProvider.DeleteCategoryArtikel(_articleCategory);
            }
        }
        #endregion
    }
}
