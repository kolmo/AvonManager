using AvonManager.BusinessObjects;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Prism.Mvvm;
using System;
using System.Runtime.CompilerServices;

namespace AvonManager.ArtikelModule.Views
{
    public class CategoryEditViewModel : BindableBase
    {
        private struct BackingFields
        {
            public int KategorieId;
            public string Name;
            public string Bemerkung;
        }
        #region Private fields
        private BackingFields bFields;
        private BackingFields clone;
        IKategorieProvider _kategorienProvider;
        private KategorieDto _category;
        #endregion
        public CategoryEditViewModel(KategorieDto category, IKategorieProvider provider)
        {
            _kategorienProvider = provider;
            _category = category;
            InitProperties();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name
        {
            get { return bFields.Name; }
            set { SetProperty(ref bFields.Name, value); }
        }

        /// <summary>
        /// Gets or sets the Bemerkung.
        /// </summary>
        /// <value>
        /// The Bemerkung.
        /// </value>
        public string Bemerkung
        {
            get { return bFields.Bemerkung; }
            set { SetProperty(ref bFields.Bemerkung, value); }
        }

        /// <summary>
        /// Gets or sets the CategoryId.
        /// </summary>
        /// <value>
        /// The CategoryId.
        /// </value>
        public int CategoryId
        {
            get { return bFields.KategorieId; }
            set { SetProperty(ref bFields.KategorieId, value); }
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
            set
            {
                if (_articleCount!=value)
                {
                    _articleCount = value;
                    RaisePropertyChanged(nameof(ArticleCount));
                }
            }
        }

        #endregion
        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            SaveCategory();
            return ok;
        }

        #endregion

        private void InitProperties()
        {
            bFields.KategorieId = _category.KategorieId;
            bFields.Name = _category.Name;
            bFields.Bemerkung = _category.Bemerkung;
            clone = bFields;
        }
        private void SaveCategory()
        {
            if (_category != null)
            {
                _category.KategorieId = CategoryId;
                _category.Name = Name;
                _category.Bemerkung = Bemerkung;
                try
                {
                    _kategorienProvider.SaveKategorie(_category);
                    clone = bFields;
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                }
            }
        }
    }
}
