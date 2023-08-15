using AvonManager.Common.Base;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.Views
{
    public class CategorySearchViewModel : ErrorAwareBaseViewModel
    {
        private const string LOAD = "LOAD";
        private List<CategoryEditViewModel> _categoryList;
        IKategorieProvider _kategorieDataProvider;
        public CategorySearchViewModel(IKategorieProvider kategorieDataProvider,
               IEventAggregator eventAggregator
             , BusyFlagsManager bFlagsManager) : base(bFlagsManager, eventAggregator)
        {
            _kategorieDataProvider = kategorieDataProvider;
            AddNewKategorieCommand = new DelegateCommand(AddNewKategorieAction);
            DeleteKategorieCommand = new DelegateCommand<CategoryEditViewModel>(DeleteKategorieAction);
        }

        private ObservableCollection<CategoryEditViewModel> _filteredCategories;
        /// <summary>
        /// Gets or sets the FilteredCategories.
        /// </summary>
        /// <value>
        /// The FilteredCategories.
        /// </value>
        public ObservableCollection<CategoryEditViewModel> FilteredCategories
        {
            get { return _filteredCategories; }
            set { SetProperty(ref _filteredCategories, value); }
        }
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();

        public ICommand AddNewKategorieCommand { get; }
        public ICommand DeleteKategorieCommand { get; }

        public async void LoadData()
        {
            Logger.Current.Write("Lade Kategoriedaten");
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var liste = await _kategorieDataProvider.ListAllKategorien();
                _categoryList = new List<CategoryEditViewModel>();
                foreach (var item in liste)
                {
                    CategoryEditViewModel vm = new CategoryEditViewModel(item, _kategorieDataProvider);
                    _categoryList.Add(vm);
                }
                FilteredCategories = new ObservableCollection<CategoryEditViewModel>(_categoryList);
                var categoryIds = liste.Select(x => x.KategorieId).ToArray();
                var dic = await _kategorieDataProvider.ListArticleCountsByCategory(categoryIds);
                if (dic?.Count > 0)
                {
                    foreach (var categoryViewModel in FilteredCategories)
                    {
                        if (dic.ContainsKey(categoryViewModel.CategoryId))
                        {
                            categoryViewModel.ArticleCount = dic[categoryViewModel.CategoryId];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            finally
            {
                BusyFlagsMgr.DecBusyFlag(LOAD);
            }
        }
        private void AddNewKategorieAction()
        {
            var neueKat = new BusinessObjects.KategorieDto { Name = "Neue Kategorie" };
            try
            {
                neueKat.KategorieId = _kategorieDataProvider.AddKategorie(neueKat);
                CategoryEditViewModel vm = new CategoryEditViewModel(neueKat, _kategorieDataProvider);
                FilteredCategories.Insert(0, vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void DeleteKategorieAction(object obj)
        {
            CategoryEditViewModel category = obj as CategoryEditViewModel;
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation() {
                Title = "Nachfrage",
                Content = $"Soll die Katgorie '{category.Name}' wirklich gelöscht werden?",
             Entity = category
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteCategoryFromDb);

        }
        private void DeleteCategoryFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                CategoryEditViewModel vm = confirmation.Entity as CategoryEditViewModel;
                try
                {
                    _kategorieDataProvider.DeleteKategorie(vm.CategoryId);
                    FilteredCategories.Remove(vm);

                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
    }
}
