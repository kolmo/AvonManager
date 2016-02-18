using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace AvonManager.Bestellungen.Common
{
    public class NewOrderConfirmation : Confirmation
    {
        public CustomerListEntry Customer { get; set; }
    }
}
