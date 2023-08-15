
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.Notifications
{
    public class AssignmentSelectionNotification : Confirmation, IInteractionRequestAware
    {
        public AssignmentSelectionNotification(IEnumerable<object> items)
        {
            foreach(object item in items)
            {
                this.ArtikelAssignments.Add(item);
            }
            CloseCommand = new DelegateCommand(CloseAction);
        }
        public List<object> ArtikelAssignments { get; } = new List<object>();
        public ICommand CloseCommand { get; set; }

        public Action FinishInteraction
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public INotification Notification
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        private void CloseAction()
        {
            Confirmed = true;
            FinishInteraction();
        }
    }
}
