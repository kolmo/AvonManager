using AvonManager.ArtikelModule.Notifications;
using AvonManager.ArtikelModule.ViewModels;
using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.Views
{
    public class MarkingSearchViewModel : ErrorAwareBaseViewModel
    {
        IMarkierungenDataProvider _markierungenDataProvider;
        IRegionManager _regionManager;
        public MarkingSearchViewModel(IMarkierungenDataProvider markierungenDataProvider, IRegionManager regionManager
              , IEventAggregator eventAggregator):base(eventAggregator)
        {
            _markierungenDataProvider = markierungenDataProvider;
            _regionManager = regionManager;
            AddNewMarkingCommand = new DelegateCommand(AddNewMarkingAction);
            DeleteMarkingCommand = new DelegateCommand<MarkingListEntryViewModel>(DeleteMarkingAction);
            EditMarkingCommand = new DelegateCommand<MarkingListEntryViewModel>(EditMarkingAction);
            EventAggregator.GetEvent<MarkingChangedEvent>().Subscribe(async x => await BuildMarkingList());
        }
        #region Properties
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public ObservableCollection<MarkingListEntryViewModel> Markierungen { get; } = new ObservableCollection<MarkingListEntryViewModel>();

        public ICommand AddNewMarkingCommand { get; }
        public ICommand DeleteMarkingCommand { get; }
        public ICommand EditMarkingCommand { get; }
        #endregion

        #region Public methods
        public async void LoadData()
        {
            await BuildMarkingList();
        }
        #endregion

        #region Private methods
        private async Task BuildMarkingList()
        {
            try
            {
                var liste = await _markierungenDataProvider.ListAllMarkierungen(EntitaetDto.Artikel);
                Markierungen.Clear();
                foreach (var item in liste)
                {
                    MarkingListEntryViewModel vm = new MarkingListEntryViewModel(item, EditMarkingAction, DeleteMarkingAction);
                    Markierungen.Add(vm);
                }
                var idArray = liste.Select(x => x.MarkierungId).ToArray();
                var dic = await _markierungenDataProvider.ListArticleCountsByMarkingIds(idArray);
                if (dic?.Count > 0)
                {
                    foreach (var markingViewModel in Markierungen)
                    {
                        if (dic.ContainsKey(markingViewModel.MarkierungId))
                        {
                            markingViewModel.ArticleCount = dic[markingViewModel.MarkierungId];
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
        private void AddNewMarkingAction()
        {
            var neueMark = new MarkierungDto { Titel = "Neue Markierung", EntitaetId = 1 };
            try
            {
                neueMark.MarkierungId = _markierungenDataProvider.AddMarking(neueMark);
                MarkingListEntryViewModel vm = new MarkingListEntryViewModel(neueMark, EditMarkingAction, DeleteMarkingAction);
                Markierungen.Insert(0, vm);
                EditMarkingAction(vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void DeleteMarkingAction(MarkingListEntryViewModel marking)
        {
            DeleteConfirmation deleteConfirmation = new DeleteConfirmation()
            {
                Title = "Nachfrage",
                Content = $"Soll die Markierung '{marking.Titel}' wirklich gelöscht werden?",
                Entity = marking
            };
            DeleteEntityRequest.Raise(deleteConfirmation, DeleteMarkingFromDb);
        }
        private void DeleteMarkingFromDb(DeleteConfirmation confirmation)
        {
            if (confirmation?.Confirmed == true)
            {
                MarkingListEntryViewModel vm = confirmation.Entity as MarkingListEntryViewModel;
                try
                {
                    _markierungenDataProvider.DeleteMarking(vm.MarkierungId);
                    int listPosition = Markierungen.IndexOf(vm);
                    int listLength = Markierungen.Count;
                    Markierungen.Remove(vm);
                    if (listPosition + 1 < listLength)
                    {
                        EditMarkingAction(Markierungen[listPosition]);
                    }
                    else if (Markierungen.Any())
                    {
                        EditMarkingAction(Markierungen.Last());
                    }
                    else
                    {
                        EditMarkingAction(null);
                    }

                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        private void EditMarkingAction(MarkingListEntryViewModel marking)
        {         
                NavigationParameters pars = new NavigationParameters();
                pars.Add("marking", marking);
                var moduleAWorkspace = new Uri("MarkingEditView", UriKind.Relative);
                _regionManager.RequestNavigate("MarkingDetailsRegion", moduleAWorkspace, pars);
        }

        #endregion
    }
}
