using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            DeleteMarkingCommand = new DelegateCommand<MarkingEditViewModel>(DeleteMarkingAction);
        }
        #region Properties
        public InteractionRequest<DeleteConfirmation> DeleteEntityRequest { get; } = new InteractionRequest<DeleteConfirmation>();
        public ObservableCollection<MarkingEditViewModel> Markierungen { get; } = new ObservableCollection<MarkingEditViewModel>();

        public ICommand AddNewMarkingCommand { get; }
        public ICommand DeleteMarkingCommand { get; }
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
                    MarkingEditViewModel vm = new MarkingEditViewModel(item, _markierungenDataProvider);
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
                MarkingEditViewModel vm = new MarkingEditViewModel(neueMark, _markierungenDataProvider);
                Markierungen.Insert(0, vm);
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
        }
        private void DeleteMarkingAction(MarkingEditViewModel marking)
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
                MarkingEditViewModel vm = confirmation.Entity as MarkingEditViewModel;
                try
                {
                    _markierungenDataProvider.DeleteMarking(vm.MarkierungId);
                    int listPosition = Markierungen.IndexOf(vm);
                    int listLength = Markierungen.Count;
                    Markierungen.Remove(vm);
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
