using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvonManager.ArtikelModule.Views
{
    public class CategoryEditViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private struct BackingFields
        {
            public int KategorieId;
            public string Name;
            public string Bemerkung;
            public byte[] Logo;
        }
        #region Private fields
        private BackingFields bFields;
        private BackingFields clone;
        private bool _isInitializing = false;
        IKategorieProvider _kategorienProvider;
        private KategorieDto _category;
        #endregion
        public CategoryEditViewModel(IKategorieProvider provider,
                       IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _kategorienProvider = provider;
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

        /// <summary>
        /// Gets or sets the Logo.
        /// </summary>
        /// <value>
        /// The Logo.
        /// </value>
        public byte[] Logo
        {
            get { return bFields.Logo; }
            set { SetProperty(ref bFields.Logo, value); }
        }

        #endregion
        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
                SaveCategory();
            return ok;
        }

        #endregion
        private async void LoadCategory(int categoryId)
        {
            _isInitializing = true;
            try
            {
                _category = await _kategorienProvider.LoadCategoryById(categoryId);
                InitProperties();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            _isInitializing = false;
        }
        private void InitProperties()
        {
            CategoryId = _category.KategorieId;
            Name = _category.Name;
            Bemerkung = _category.Bemerkung;
            Logo = _category.Logo;
            clone = bFields;
        }
        private void SaveCategory()
        {
            if (_category!= null)
            {
                _category.KategorieId = CategoryId;
                _category.Name = Name;
                _category.Bemerkung = Bemerkung;
                _category.Logo = Logo;
                try
                {
                    _kategorienProvider.SaveKategorie(_category);
                    EventAggregator.GetEvent<CategoryChangedEvent>().Publish(new CategoryChangedEventArgs { Category = _category, ChangedType = ChangedType.Update });
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
          
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var category = navigationContext.Parameters.FirstOrDefault();
            int categoryID = (int)category.Value;
            LoadCategory(categoryID);
        }
    }
}
