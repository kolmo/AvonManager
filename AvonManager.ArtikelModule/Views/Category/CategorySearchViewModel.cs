using AvonManager.ArtikelModule.ViewModels;
using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.Views
{
    public class CategorySearchViewModel : ErrorAwareBaseViewModel
    {
        private const string ALLCHAR = "#";
        private const string LOAD = "LOAD";
        private string _currentInitial;
        private List<CategoryListEntryViewModel> _categoryList;
        IKategorieProvider _kategorieDataProvider;
        IRegionManager _regionManager;
        public CategorySearchViewModel(IKategorieProvider kategorieDataProvider,
            IRegionManager regionManager
              , IEventAggregator eventAggregator
             , BusyFlagsManager bFlagsManager) : base(bFlagsManager, eventAggregator)
        {
            _kategorieDataProvider = kategorieDataProvider;
            _regionManager = regionManager;
            AddNewKategorieCommand = new DelegateCommand(AddNewKategorieAction);
            DeleteKategorieCommand = new DelegateCommand<CategoryListEntryViewModel>(DeleteKategorieAction);
            EditCategoryCommand = new DelegateCommand<CategoryListEntryViewModel>(EditCategoryAction);
            FilterInitialsCommand = new DelegateCommand<string>(FilterInitialsAction);
            EventAggregator.GetEvent<CategoryChangedEvent>().Subscribe(RefreshCategory);
        }

        private ObservableCollection<CategoryListEntryViewModel> _filteredCategories;
        /// <summary>
        /// Gets or sets the FilteredCategories.
        /// </summary>
        /// <value>
        /// The FilteredCategories.
        /// </value>
        public ObservableCollection<CategoryListEntryViewModel> FilteredCategories
        {
            get { return _filteredCategories; }
            set { SetProperty(ref _filteredCategories, value); }
        }
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();

        public List<string> InitialsList { get; private set; }
        public ICommand AddNewKategorieCommand { get; }
        public ICommand DeleteKategorieCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public DelegateCommand<string> FilterInitialsCommand { get; private set; }

        public async void LoadData()
        {
            Logger.Current.Write("Lade Kategoriedaten");
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var liste = await _kategorieDataProvider.ListAllKategorien();
                _categoryList = new List<CategoryListEntryViewModel>();
                foreach (var item in liste)
                {
                    CategoryListEntryViewModel vm = new CategoryListEntryViewModel(item, EditCategoryAction, DeleteKategorieAction);
                    _categoryList.Add(vm);
                }
                InitialsList = liste.Where(x => x.Name?.Trim().Length > 0).Select(x => x.Name.Trim().ToUpper().Substring(0, 1)).Distinct().OrderBy(x => x).ToList();
                InitialsList.Insert(0, ALLCHAR);
                OnPropertyChanged(nameof(InitialsList));
                FilterInitialsAction(ALLCHAR);
                var categoryIds = liste.Select(x => x.KategorieId).ToArray();
                var dic = await _kategorieDataProvider.ListArticleCountsByCategory(categoryIds);
                if (dic?.Count > 0)
                {
                    foreach (var categoryViewModel in FilteredCategories)
                    {
                        if (dic.ContainsKey(categoryViewModel.KategorieId))
                        {
                            categoryViewModel.ArticleCount = dic[categoryViewModel.KategorieId];
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
        private async void RefreshCategory(CategoryChangedEventArgs args)
        {
            KategorieDto category = args.Category;
            var cat = _categoryList.FirstOrDefault(x => x.KategorieId == category.KategorieId);
            if (cat != null)
            {
                CategoryListEntryViewModel vm = new CategoryListEntryViewModel(category, EditCategoryAction, DeleteKategorieAction);
                int idx = _categoryList.IndexOf(cat);
                _categoryList[idx] = vm;
                FilterInitialsAction(_currentInitial);
                try
                {
                    var dic = await _kategorieDataProvider.ListArticleCountsByCategory(new int[] { cat.KategorieId });
                    vm.ArticleCount = dic[vm.KategorieId];

                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void AddNewKategorieAction()
        {
            var neueKat = new BusinessObjects.KategorieDto { Name = "Neue Kategorie" };
            try
            {
                neueKat.KategorieId = _kategorieDataProvider.AddKategorie(neueKat);
                CategoryListEntryViewModel vm = new CategoryListEntryViewModel(neueKat, EditCategoryAction, DeleteKategorieAction);
                FilteredCategories.Insert(0, vm);
                EditCategoryAction(vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void DeleteKategorieAction(object obj)
        {
            CategoryListEntryViewModel category = obj as CategoryListEntryViewModel;
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation() {
                Title = "Nachfrage",
                Content = $"Soll die Katgorie '{category.Kategoriename}' wirklich gelöscht werden?",
             Entity = category
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteCategoryFromDb);

        }
        private void DeleteCategoryFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                CategoryListEntryViewModel vm = confirmation.Entity as CategoryListEntryViewModel;
                try
                {
                    _kategorieDataProvider.DeleteKategorie(vm.KategorieId);
                    FilteredCategories.Remove(vm);

                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void EditCategoryAction(object obj)
        {
            CategoryListEntryViewModel category = obj as CategoryListEntryViewModel;
            if (category != null)
            {
                NavigationParameters pars = new NavigationParameters();
                pars.Add("category", category.KategorieId);
                var moduleAWorkspace = new Uri("CategoryEditView", UriKind.Relative);
                _regionManager.RequestNavigate("CategoryDetailsRegion", moduleAWorkspace, pars);
            }
        }
        private void FilterInitialsAction(string initial)
        {
            _currentInitial = initial;
            if (initial?.Equals(ALLCHAR) == true)
            {
                FilteredCategories = new ObservableCollection<CategoryListEntryViewModel>(_categoryList);
            }
            else
            {
                FilteredCategories = new ObservableCollection<CategoryListEntryViewModel>(_categoryList.Where(x => x.Kategoriename?.StartsWith(initial) == true));
            }
        }
    }
}
