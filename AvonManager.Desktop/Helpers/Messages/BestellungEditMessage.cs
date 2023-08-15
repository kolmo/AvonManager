using AvonManager.BusinessObjects;
using Prism.Events;

namespace AvonManager.Helpers.Messages
{
    public class BestellungEditMessage : PubSubEvent<BestellungDto>
    {
        public bool IsEditing { get; set; }
    }
}