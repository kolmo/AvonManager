using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.Common.Events
{
    /// <summary>
    /// A composite Presentation event 
    /// </summary>
    public class NavigationCompletedEvent : PubSubEvent<string>
    {
    }
}