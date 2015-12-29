using AvonManager.Common.Helpers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;

namespace AvonManager.Common.Base
{
    public class ErrorAwareBaseViewModel : BindableBase
    {
        #region Private fields
        BusyFlagsManager _busyFlagsManager;

        #endregion
        public InteractionRequest<Notification> ShowErrorRequest { get; private set; } = new InteractionRequest<Notification>();
        public ErrorAwareBaseViewModel()
        {

        }
        protected ErrorAwareBaseViewModel(BusyFlagsManager busyFlagsManager)
        {
            _busyFlagsManager = busyFlagsManager;
        }
        #region Properties
        public BusyFlagsManager BusyFlagsMgr { get { return _busyFlagsManager; } }

        #endregion
        protected void ShowException(Exception exception)
        {
            Notification notification = new Notification
            {
                Content = exception.ToString(),
                Title = "Error"
            };
            ShowErrorRequest.Raise(notification);
        }
    }
}
