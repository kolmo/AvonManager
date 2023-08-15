using AvonManager.ArtikelModule.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class MarkierungenSelectViewModel : BindableBase, IInteractionRequestAware
    {
        private AssignmentSelectionNotification notification;

        public MarkierungenSelectViewModel()
        {
            CloseCommand = new DelegateCommand(CloseAction);
        }
        public ICommand CloseCommand { get; set; }
        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get
            {
                return this.notification;
            }

            set
            {
                if (value is AssignmentSelectionNotification)
                {
                    // To keep the code simple, this is the only property where we are raising the PropertyChanged event,
                    // as it's required to update the bindings when this property is populated.
                    // Usually you would want to raise this event for other properties too.
                    this.notification = value as AssignmentSelectionNotification;
                    AlleMarkierungen.Clear();
                    notification.ArtikelAssignments.ForEach(x =>
                    {
                        AlleMarkierungen.Add(x);
                    });
                    
                    OnPropertyChanged(() => Notification);
                    OnPropertyChanged(() => SortedListe);
                }
            }
        }
        public ObservableCollection<object> AlleMarkierungen { get; } = new ObservableCollection<object>();

        public ObservableCollection<object> SortedListe
        {
            get { return AlleMarkierungen; }
        }

        private object _checkedEntry;
        /// <summary>
        /// Gets or sets the CheckedEntry.
        /// </summary>
        /// <value>
        /// The CheckedEntry.
        /// </value>
        public object CheckedEntry
        {
            get { return _checkedEntry; }
            set { SetProperty(ref _checkedEntry, value); }
        }

        private void CloseAction()
        {
            notification.Confirmed = true;
            FinishInteraction();
        }    
    }
}
