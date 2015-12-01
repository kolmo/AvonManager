using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.Helpers.Messages
{
    public class BestellungEditMessage : PubSubEvent<BestellungDto>
    {
        public bool IsEditing { get; set; }
    }
}
