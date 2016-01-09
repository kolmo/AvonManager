using AvonManager.Common.Helpers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;

namespace AvonManager.Common.Base
{
    public class ErrorAwareBaseViewModel : BindableBase
    {
        #region Private fields
        BusyFlagsManager _busyFlagsManager;
        IEventAggregator _eventAggregator;
        #endregion
        public InteractionRequest<Notification> ShowErrorRequest { get; private set; } = new InteractionRequest<Notification>();
        public ErrorAwareBaseViewModel()
        {

        }
        protected ErrorAwareBaseViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        protected ErrorAwareBaseViewModel(
            BusyFlagsManager busyFlagsManager,
            IEventAggregator eventAggregator):this(eventAggregator)
        {
            _busyFlagsManager = busyFlagsManager;
        }
        #region Properties
        public BusyFlagsManager BusyFlagsMgr { get { return _busyFlagsManager; } }
        protected IEventAggregator EventAggregator { get { return _eventAggregator; } }
        #endregion
        protected void ShowException(Exception exception)
        {
            Notification notification = new Notification
            {
                Content = exception.ToString(),
                Title = "Fehler"
            };
            ShowErrorRequest.Raise(notification);
        }
    }
}
