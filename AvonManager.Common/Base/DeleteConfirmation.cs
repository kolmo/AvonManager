using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace AvonManager.Common.Base
{
    public class DeleteConfirmation : Confirmation
    {
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; }
    }
}
