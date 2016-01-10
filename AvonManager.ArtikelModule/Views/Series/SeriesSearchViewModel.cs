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
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.Views
{
    public class SeriesSearchViewModel : ErrorAwareBaseViewModel
    {
        private const string ALLCHAR = "#";
        private const string LOAD = "LOAD";
        private string _currentInitial;
        private List<SeriesListEntryViewModel> _seriesList;
        ISerienDataProvider _serienDataProvider;
        IRegionManager _regionManager;
        public SeriesSearchViewModel(ISerienDataProvider serienDataProvider, 
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            BusyFlagsManager bFlagsManager
            ):base(bFlagsManager, eventAggregator)
        {
            _serienDataProvider = serienDataProvider;
            _regionManager = regionManager;
            AddNewSerieCommand = new DelegateCommand(AddNewSerieAction);
            DeleteSerieCommand = new DelegateCommand<SeriesListEntryViewModel>(DeleteSerieAction);
            EditSerieCommand = new DelegateCommand<SeriesListEntryViewModel>(EditSeriesAction);
            FilterInitialsCommand = new DelegateCommand<string>(FilterInitialsAction);
            EventAggregator.GetEvent<SeriesChangedEvent>().Subscribe(async x => await BuildSeriesList());
        }
        #region Properties

        private ObservableCollection<SeriesListEntryViewModel> _filteredSeries;
        /// <summary>
        /// Gets or sets the FilteredSeries.
        /// </summary>
        /// <value>
        /// The FilteredSeries.
        /// </value>
        public ObservableCollection<SeriesListEntryViewModel> FilteredSeries
        {
            get { return _filteredSeries; }
            set { SetProperty(ref _filteredSeries, value); }
        }

        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public List<string> InitialsList { get; private set; }

        public ICommand AddNewSerieCommand { get; }
        public ICommand DeleteSerieCommand { get; }
        public DelegateCommand<string> FilterInitialsCommand { get; private set; }
        public ICommand EditSerieCommand { get; }

        #endregion
        #region Public Methods

        public async void LoadData()
        {
            Logger.Current.Write("Lade Seriendaten");
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                await BuildSeriesList();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
            }
            finally
            {
                BusyFlagsMgr.DecBusyFlag(LOAD);
            }
        }
        #endregion

        #region Private methods

        private async Task BuildSeriesList()
        {
            try
            {
                var liste = await _serienDataProvider.ListAllSerien();
                _seriesList = new List<SeriesListEntryViewModel>();
                foreach (var item in liste.OrderBy(x => x.Name))
                {
                    _seriesList.Add(new SeriesListEntryViewModel(item, EditSeriesAction, DeleteSerieAction));
                }
                InitialsList = liste.Where(x => x.Name?.Trim().Length > 0).Select(x => x.Name.Trim().ToUpper().Substring(0, 1)).Distinct().OrderBy(x => x).ToList();
                InitialsList.Insert(0, ALLCHAR);
                OnPropertyChanged(nameof(InitialsList));
                string initial = _currentInitial ?? ALLCHAR;
                FilterInitialsAction(initial);
                var idArray = liste.Select(x => x.SerienId).ToArray();
                var dic = await _serienDataProvider.ListArticleCountsBySeriesIds(idArray);
                if (dic?.Count > 0)
                {
                    foreach (var seriesViewModel in _seriesList)
                    {
                        if (dic.ContainsKey(seriesViewModel.SerienId.Value))
                        {
                            seriesViewModel.ArticleCount = dic[seriesViewModel.SerienId.Value];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void AddNewSerieAction()
        {
            var neueSerie = new BusinessObjects.SerieDto { Name = "Neue Serie" };
            try
            {
                neueSerie.SerienId = _serienDataProvider.AddSerie(neueSerie);
                SeriesListEntryViewModel vm = new SeriesListEntryViewModel(neueSerie, EditSeriesAction, DeleteSerieAction);
                FilteredSeries.Insert(0, vm);
                EditSeriesAction(vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void DeleteSerieAction(SeriesListEntryViewModel series)
        {
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation() {
                Title = "Nachfrage",
                Content = $"Soll die Serie '{series.Name}' wirklich gelöscht werden?",
                Entity = series
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteSeriesFromDb);
        }
        private void DeleteSeriesFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                SeriesListEntryViewModel vm = confirmation.Entity as SeriesListEntryViewModel;
                try
                {
                    _serienDataProvider.DeleteSerie(vm.SerienId.Value);
                    FilteredSeries.Remove(vm);
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void EditSeriesAction(SeriesListEntryViewModel series)
        {
            if (series != null)
            {
                NavigationParameters pars = new NavigationParameters();
                pars.Add("series", series.SerienId);
                var moduleAWorkspace = new Uri("SeriesEditView", UriKind.Relative);
                _regionManager.RequestNavigate("SeriesDetailsRegion", moduleAWorkspace, pars);
            }
        }
        private void FilterInitialsAction(string initial)
        {
            _currentInitial = initial;
            if (initial?.Equals(ALLCHAR) == true)
            {
                FilteredSeries = new ObservableCollection<SeriesListEntryViewModel>(_seriesList);
            }
            else
            {
                FilteredSeries = new ObservableCollection<SeriesListEntryViewModel>(_seriesList.Where(x => x.Name?.StartsWith(initial) == true));
            }
        }
        #endregion
    }
}
