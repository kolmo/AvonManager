using Prism.Interactivity.InteractionRequest;

namespace AvonManager.Common.Base
{
    public class TakePictureConfirmation : Confirmation
    {
        public byte[] ImageData { get; set; }
    }
}
