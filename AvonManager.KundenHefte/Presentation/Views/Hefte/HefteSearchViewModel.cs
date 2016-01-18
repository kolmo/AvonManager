using System.Linq;
using System.Collections.ObjectModel;
using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using System;
using AvonManager.Interfaces.Criteria;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;
using AvonManager.Common.Events;
using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.KundenHefte.ViewModels
{
    public class HefteSearchViewModel : ErrorAwareBaseViewModel
    {
        #region Private fields
        private const string LOAD = "LOAD";
        IHefteDataProvider _dataProvider;
        private ObservableCollection<HeftViewModel> _alleHefte;
        private IRegionManager _regionManager;
        private readonly IBrochureSearchCriteria _brochureSearchCriteria;
        #endregion
        public HefteSearchViewModel()
        {
        }
        public HefteSearchViewModel(IHefteDataProvider dataProvider, IRegionManager regionManager,
            IBrochureSearchCriteria brochureSearchCriteria,
             IEventAggregator eventAggregator,
            BusyFlagsManager busyFlagsManager):base(busyFlagsManager, eventAggregator)
        {
            _dataProvider = dataProvider;
            _regionManager = regionManager;
            _brochureSearchCriteria = brochureSearchCriteria;
            StarSearchCommand = new DelegateCommand(StartSearch);
            AddBrochureCommand = new DelegateCommand(AddBrochureAction);
            EventAggregator.GetEvent<BrochureChangedEvent>().Subscribe(x => LoadData());
        }
        #region Properties
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public IBrochureSearchCriteria Criteria { get { return _brochureSearchCriteria; } }
        public ICommand StarSearchCommand { get; private set; }
        public ICommand ResetSearchCommand { get; private set; }
        public DelegateCommand AddBrochureCommand { get; private set; }
        /// <summary>
        /// Gets or sets the AlleKategorien.
        /// </summary>
        /// <value>
        /// The AlleKategorien.
        /// </value>
        public ObservableCollection<HeftViewModel> AlleHefte
        {
            get { return _alleHefte; }
            set
            {
                if (_alleHefte != value)
                {
                    _alleHefte = value;
                    OnPropertyChanged(() => this.AlleHefte);
                }
            }
        }
        #endregion
        public void LoadData()
        {
            StartSearch();
        }
        private async void StartSearch()
        {
            BusyFlagsMgr.ResetAllBusyFlags();
            BusyFlagsMgr.IncBusyFlag(LOAD);
            try
            {
                var result = await _dataProvider.SearchBrochures(_brochureSearchCriteria);
                AlleHefte = new ObservableCollection<HeftViewModel>();
                foreach (HeftDto heft in result)
                {
                    HeftViewModel vm = new HeftViewModel(heft, EditBrochureAction, DeleteBrochureAction);
                    AlleHefte.Add(vm);
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

        private void EditBrochureAction(HeftViewModel brochure)
        {
            NavigationParameters pars = new NavigationParameters();
            pars.Add("brochure", brochure);
            var moduleAWorkspace = new Uri("HeftEditView", UriKind.Relative);
            _regionManager.RequestNavigate(AvonManager.Common.RegionNames.BrochureDetailsRegion, moduleAWorkspace, pars);
        }
        private void DeleteBrochureAction(HeftViewModel brochure)
        {
            if (brochure != null)
            {
                DeleteConfirmation deleteConfirmation = new DeleteConfirmation()
                {
                    Title = "Nachfrage",
                    Content = $"Soll das Heft '{brochure.Titel}' wirklich gelöscht werden?",
                    Entity = brochure
                };
                DeleteEntityRequest.Raise(deleteConfirmation, DeleteBrochureFromDb);
            }
        }
        private void DeleteBrochureFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                HeftViewModel brochure = confirmation.Entity as HeftViewModel;
                int listPosition = AlleHefte.IndexOf(brochure);
                int listLength = AlleHefte.Count;
                try
                {
                    _dataProvider.DeleteHeft(brochure.HeftId);
                    AlleHefte.Remove(brochure);
                    if (listPosition + 1 < listLength)
                    {
                        EditBrochureAction(AlleHefte[listPosition]);
                    }
                    else if (AlleHefte.Any())
                    {
                        EditBrochureAction(AlleHefte.Last());
                    }
                    else
                    {
                        EditBrochureAction(null);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }

        private void AddBrochureAction()
        {
            var newBrochure = new HeftDto { Titel = "Neues Heft", Jahr = DateTime.Now.Year };
            try
            {
                newBrochure.HeftId = _dataProvider.AddHeft(newBrochure);
                HeftViewModel vm = new HeftViewModel(newBrochure, EditBrochureAction, DeleteBrochureAction);
                AlleHefte.Insert(0, vm);
                EditBrochureAction(vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
    }
}
