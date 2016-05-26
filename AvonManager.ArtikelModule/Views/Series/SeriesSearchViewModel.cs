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
        private const string LOAD = "LOAD";
        private List<SeriesEditViewModel> _seriesList;
        ISerienDataProvider _serienDataProvider;
        public SeriesSearchViewModel(ISerienDataProvider serienDataProvider, 
            IEventAggregator eventAggregator,
            BusyFlagsManager bFlagsManager
            ):base(bFlagsManager, eventAggregator)
        {
            _serienDataProvider = serienDataProvider;
            AddNewSerieCommand = new DelegateCommand(AddNewSerieAction);
            DeleteSerieCommand = new DelegateCommand<SeriesEditViewModel>(DeleteSerieAction);
        }
        #region Properties

        private ObservableCollection<SeriesEditViewModel> _filteredSeries;
        /// <summary>
        /// Gets or sets the FilteredSeries.
        /// </summary>
        /// <value>
        /// The FilteredSeries.
        /// </value>
        public ObservableCollection<SeriesEditViewModel> FilteredSeries
        {
            get { return _filteredSeries; }
            set { SetProperty(ref _filteredSeries, value); }
        }

        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();

        public ICommand AddNewSerieCommand { get; }
        public ICommand DeleteSerieCommand { get; }

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
                _seriesList = new List<SeriesEditViewModel>();
                foreach (var item in liste.OrderBy(x => x.Name))
                {
                    _seriesList.Add(new SeriesEditViewModel(item, _serienDataProvider, EventAggregator));
                }
                FilteredSeries = new ObservableCollection<SeriesEditViewModel>(_seriesList);
                var idArray = liste.Select(x => x.SerienId).ToArray();
                var dic = await _serienDataProvider.ListArticleCountsBySeriesIds(idArray);
                if (dic?.Count > 0)
                {
                    foreach (var seriesViewModel in _seriesList)
                    {
                        if (dic.ContainsKey(seriesViewModel.SeriesId))
                        {
                            seriesViewModel.ArticleCount = dic[seriesViewModel.SeriesId];
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
                SeriesEditViewModel vm = new SeriesEditViewModel(neueSerie, _serienDataProvider, EventAggregator);
                FilteredSeries.Insert(0, vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void DeleteSerieAction(SeriesEditViewModel series)
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
                SeriesEditViewModel vm = confirmation.Entity as SeriesEditViewModel;
                try
                {
                    _serienDataProvider.DeleteSerie(vm.SeriesId);
                    FilteredSeries.Remove(vm);
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        #endregion
    }
}
