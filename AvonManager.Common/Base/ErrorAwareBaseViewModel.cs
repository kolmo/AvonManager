using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;

namespace AvonManager.Common.Base
{
    public class ErrorAwareBaseViewModel : BindableBase
    {
        public InteractionRequest<Notification> ShowErrorRequest { get; private set; } = new InteractionRequest<Notification>();

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
