using AvonManager.ArtikelModule.Notifications;
using AvonManager.ArtikelModule.Views;
using AvonManager.BusinessObjects;
using AvonManager.Common;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using AvonManager.Interfaces.Criteria;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvonManager.ArtikelModule.ViewModels
{

    public class ArtikelSearchViewModel : ErrorAwareBaseViewModel
    {
        private const string LOAD = "LOAD";
        int _page = 0;
        private SeriesListEntryViewModel _selectedSeries;
        private CategoryListEntryViewModel _selectedCategory;
        private List<MarkierungDto> _allMarkings;
        IArtikelDataProvider _dataProvider;
        IMarkierungenDataProvider _markierungenDataProvider;
        IKategorieProvider _kategorienDataProvider;
        ISerienDataProvider _seriendataProvider;
        IRegionManager _regionManager;
        IArticleSearchCriteria _articleSearchCriteria;
        public ArtikelSearchViewModel(IArtikelDataProvider dataProvider,
            IMarkierungenDataProvider markierungenDataProvider,
            IKategorieProvider kategorienDataProvider,
            ISerienDataProvider seriendataProvider,
            IRegionManager regionManager
            , IEventAggregator eventAggregator
            , IArticleSearchCriteria articleSearchCriteria
            , BusyFlagsManager bFlagsManager) : base(bFlagsManager, eventAggregator)
        {
            _articleSearchCriteria = articleSearchCriteria;
            _regionManager = regionManager;
            _dataProvider = dataProvider;
            _markierungenDataProvider = markierungenDataProvider;
            _kategorienDataProvider = kategorienDataProvider;
            _seriendataProvider = seriendataProvider;
            KategorienFilter = new List<CategoryListEntryViewModel>();
            MarkierungenFilter = new ObservableCollection<MarkingListEntryViewModel>();
            SerienFilter = new List<SeriesListEntryViewModel>();
            SelectArtikel = new DelegateCommand<ArticleViewModel>(SelectArtikelAction);
            DeleteArtikel = new DelegateCommand<ArticleViewModel>(DeleteArtikelAction);
            SearchArticlesCommand = new DelegateCommand(SearchArticlesAction);
            ResetFiltersCommand = new DelegateCommand(ResetFilterAction);
            LoadMoreArticlesCommand = new DelegateCommand(LoadMoreArticlesAction);
            AddNewArticleCommand = new DelegateCommand(AddNewArticleAction);
            FilterInitialsCommand = new DelegateCommand<string>(FilterSeriesInitialsAction);
            SelectCategoryCommand = new DelegateCommand<CategoryListEntryViewModel>(SelectCategoryAction);
            SelectSeriesCommand = new DelegateCommand<SeriesListEntryViewModel>(SelectSeriesAction);
            SelectMarkingCommand = new DelegateCommand<MarkingListEntryViewModel>(SelectMarkingAction);
            EventAggregator.GetEvent<ArticleChangedEvent>().Subscribe(RefreshArticle);
            EventAggregator.GetEvent<SeriesChangedEvent>().Subscribe(async x => await BuildSeriesList());
            EventAggregator.GetEvent<CategoryChangedEvent>().Subscribe(async x => await BuildCategoriesList());
            EventAggregator.GetEvent<MarkingChangedEvent>().Subscribe(async x => await BuildMarkingsList());
        }

        #region Props

        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public IArticleSearchCriteria Criteria { get { return _articleSearchCriteria; } }

        private string _busyMessage;
        /// <summary>
        /// Gets or sets the BusyMessage.
        /// </summary>
        /// <value>
        /// The BusyMessage.
        /// </value>
        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        private ObservableCollection<ArticleViewModel> _artikelListe = new ObservableCollection<ArticleViewModel>();
        /// <summary>
        /// Gets or sets the ArtikelListe.
        /// </summary>
        /// <value>
        /// The ArtikelListe.
        /// </value>
        public ObservableCollection<ArticleViewModel> ArtikelListe
        {
            get { return _artikelListe; }
            set
            {
                if (_artikelListe != value)
                {
                    _artikelListe = value;
                    OnPropertyChanged(() => this.ArtikelListe);
                }
            }
        }
        public List<string> SeriesInitialsList { get; private set; }
        public List<string> CategoryInitialsList { get; private set; }

        #endregion
        public async void LoadData()
        {
            BusyMessage = "Lade Grunddaten...";
            Logger.Current.Write("Lade daten");
            await BuildCategoriesList();
            await BuildMarkingsList();
            await BuildSeriesList();
        }

        private async Task BuildMarkingsList()
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                _allMarkings = await _markierungenDataProvider.ListAllMarkierungen(EntitaetDto.Artikel);
                MarkierungenFilter.Clear();
                foreach (var item in _allMarkings)
                {
                    var vm = new MarkingListEntryViewModel(item);
                    MarkierungenFilter.Add(vm);
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

        private async Task BuildSeriesList()
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var serien = await _seriendataProvider.ListAllSerien();
                SerienFilter = new List<SeriesListEntryViewModel>();
                foreach (var item in serien.OrderBy(x => x.Name))
                {
                    var vm = new SeriesListEntryViewModel(item);
                    SerienFilter.Add(vm);
                }
                SeriesInitialsList = serien.Where(x => x.Name?.Trim().Length > 0).Select(x => x.Name.Trim().ToUpper().Substring(0, 1)).Distinct().OrderBy(x => x).ToList();
                SeriesInitialsList.Insert(0, "#");
                OnPropertyChanged(nameof(SeriesInitialsList));
                FilteredSeriesFilter = SerienFilter;
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

        private async Task BuildCategoriesList()
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var kategorien = await _kategorienDataProvider.ListAllKategorien();
                KategorienFilter = new List<CategoryListEntryViewModel>();
                foreach (KategorieDto item in kategorien)
                {
                    var vm = new CategoryListEntryViewModel(item);
                    KategorienFilter.Add(vm);
                }
                FilteredCategoryFilter = KategorienFilter;
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

        #region Properties

        /// <summary>
        /// Todo Context fixen
        /// </summary>
        public List<SeriesListEntryViewModel> SerienFilter { get; private set; }

        private List<SeriesListEntryViewModel> _filteredSeriesFilter;
        /// <summary>
        /// Gets or sets the FilteredSeriesFilter.
        /// </summary>
        /// <value>
        /// The FilteredSeriesFilter.
        /// </value>
        public List<SeriesListEntryViewModel> FilteredSeriesFilter
        {
            get { return _filteredSeriesFilter; }
            set { SetProperty(ref _filteredSeriesFilter, value); }
        }

        private List<CategoryListEntryViewModel> _filteredCategoryFilter;
        /// <summary>
        /// Gets or sets the FilteredCategoryFilter.
        /// </summary>
        /// <value>
        /// The FilteredCategoryFilter.
        /// </value>
        public List<CategoryListEntryViewModel> FilteredCategoryFilter
        {
            get { return _filteredCategoryFilter; }
            set { SetProperty(ref _filteredCategoryFilter, value); }
        }
        /// <summary>
        /// Gets or sets the KategorienFilter.
        /// </summary>
        /// <value>
        /// Todo The KategorienFilter.
        /// </value>
        public List<CategoryListEntryViewModel> KategorienFilter { get; private set; }
        public ObservableCollection<MarkingListEntryViewModel> MarkierungenFilter { get; private set; }
        public int LoadSize { get { return 100; } }


        private bool _markierungInverted;
        /// <summary>
        /// Gets or sets the MarkierungInverted.
        /// </summary>
        /// <value>
        /// The MarkierungInverted.
        /// </value>
        public bool MarkierungInverted
        {
            get { return _markierungInverted; }
            set
            {
                if (_markierungInverted != value)
                {
                    _markierungInverted = value;
                }
            }
        }

        #region Commands
        public DelegateCommand<ArticleViewModel> SelectArtikel { get; set; }
        public DelegateCommand<CategoryListEntryViewModel> SelectCategoryCommand { get; private set; }
        public DelegateCommand<SeriesListEntryViewModel> SelectSeriesCommand { get; private set; }
        public DelegateCommand<MarkingListEntryViewModel> SelectMarkingCommand { get; set; }
        public DelegateCommand<ArticleViewModel> DeleteArtikel { get; }
        public DelegateCommand SearchArticlesCommand { get; private set; }
        public DelegateCommand ResetFiltersCommand { get; private set; }
        public DelegateCommand LoadMoreArticlesCommand { get; private set; }
        public DelegateCommand AddNewArticleCommand { get; private set; }
        public DelegateCommand<string> FilterInitialsCommand { get; private set; }
        public DelegateCommand<string> FilterCategoryInitialsCommand { get; private set; }
        #endregion

        #endregion Properties

        #region Constructors

        #endregion Constructors


        #region Callbacks/Event handler
        private async void RefreshArticle(ArticleChangedEventArgs args)
        {
            var art = ArtikelListe.FirstOrDefault(x => x.ArtikelId == args.Article.ArtikelId);
            if (art != null)
            {
                try
                {
                    var dto = await _dataProvider.LoadArtikel(art.ArtikelId);
                    ArticleViewModel vm = new ArticleViewModel(dto);
                    int idx = ArtikelListe.IndexOf(art);
                    ArtikelListe[idx] = vm;
                    await DecorateArticle(vm);
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void SearchArticlesAction()
        {
            _page = 1;
            ArtikelListe.Clear();
            LoadArticlesPage();
        }
        private async void LoadArticlesPage()
        {
            BusyMessage = "Suche Artikel...";
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var result = await _dataProvider.SearchArticles(_articleSearchCriteria, page: _page);
                AddArticlesToList(result);
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
        private async void AddArticlesToList(List<ArtikelDto> list, bool insertAtTop = false)
        {
            foreach (ArtikelDto artikel in list)
            {
                ArticleViewModel vm = new ArticleViewModel(artikel);
                if (!insertAtTop)
                    ArtikelListe.Add(vm);
                else
                    ArtikelListe.Insert(0, vm);
                await DecorateArticle(vm);
            }
        }
        private async Task DecorateArticle(ArticleViewModel article)
        {
            try
            {
                List<ArtikelMarkierungenDto> articleAssignments = await _markierungenDataProvider.ListMarkierungenByArtikel(article.ArtikelId);
                foreach (ArtikelMarkierungenDto marking in articleAssignments)
                {
                    article.Markierungen.Add(new ArticleMarkingViewModel(article.Dto, _allMarkings.FirstOrDefault(x => x.MarkierungId == marking.MarkierungId), marking, _markierungenDataProvider, true));
                }
                var series = FilteredSeriesFilter.FirstOrDefault(x => x.SerienId == article.Dto.SerienId);
                article.Serie = series?.Name;

            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private async void FilterSeriesInitialsAction(string initial)
        {
            if (initial == "#")
            {
                await BuildSeriesList();
            }
            else
            {
                FilteredSeriesFilter = SerienFilter.Where(x => x.Name.StartsWith(initial)).ToList();
            }
        }
        private void ResetFilterAction()
        {
            ArtikelListe.Clear();
            Criteria.Reset();
            _selectedSeries = null;
        }

        private void LoadMoreArticlesAction()
        {
            _page++;
            LoadArticlesPage();
        }
        private void SelectCategoryAction(CategoryListEntryViewModel category)
        {
            _page = 1;
            ResetFilterAction();
            _selectedCategory = category;
            if (category != null)
            {
                Criteria.Categories = new int[] { category.KategorieId };
            }
            else
            {
                Criteria.WithoutCategory = true;
            }
            LoadArticlesPage();
        }
        private void SelectSeriesAction(SeriesListEntryViewModel series)
        {
            _page = 1;
            ResetFilterAction();
            _selectedSeries = series;
            if (series != null)
            {
                Criteria.Series = new int[] { series.SerienId.Value };
            }
            else
            {
                Criteria.WithoutSeries = true;
            }
            LoadArticlesPage();
        }
        private void SelectMarkingAction(MarkingListEntryViewModel marking)
        {
            _page = 1;
            ResetFilterAction();
            if (marking != null)
            {
                Criteria.Markups = new int[] { marking.MarkierungId };
            }
            else
            {
                Criteria.WithoutMarkups = true;
            }
            LoadArticlesPage();
        }
        private void SelectArtikelAction(ArticleViewModel artikel)
        {
            NavigationParameters pars = new NavigationParameters();
            pars.Add("artikel", artikel);
            var moduleAWorkspace = new Uri("ArtikelDetailsWorkspace", UriKind.Relative);
            _regionManager.RequestNavigate(RegionNames.ArticleDetailsRegion, moduleAWorkspace, pars);
        }
        private void DeleteArtikelAction(ArticleViewModel artikel)
        {
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation()
            {
                Title = "Nachfrage",
                Content = $"Soll der Artikel '{artikel.Artikelname}' wirklich gelöscht werden?",
                Entity = artikel
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteArticleFromDb);
        }
        private void DeleteArticleFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                ArticleViewModel article = confirmation.Entity as ArticleViewModel;
                int listPosition = ArtikelListe.IndexOf(article);
                int listLength = ArtikelListe.Count;
                try
                {
                    _dataProvider.DeleteArticle(article.ArtikelId);
                    ArtikelListe.Remove(article);
                    if (listPosition + 1 < listLength)
                    {
                        SelectArtikelAction(ArtikelListe[listPosition]);
                    }
                    else if (ArtikelListe.Any())
                    {
                        SelectArtikelAction(ArtikelListe.Last());
                    }
                    else
                    {
                        SelectArtikelAction(null);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void AddNewArticleAction()
        {
            ArtikelDto newArticle = new ArtikelDto()
            {
                Name = string.Empty,
                SerienId = _selectedSeries?.SerienId
            };

            try
            {
                newArticle.ArtikelId = _dataProvider.AddArticle(newArticle);
                ArticleViewModel vm = new ArticleViewModel(newArticle);
                ArtikelListe.Insert(0, vm);
                SelectArtikelAction(vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        #endregion Private helper methods
    }
}