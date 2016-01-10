using System;

namespace AvonManager.BusinessObjects
{
    public class HeftKundeDto
    {
        public int HeftId { get; set; }
        public int KundenId { get; set; }
        public bool HasOrdered { get; set; }
        public DateTime? ReceivedAt { get; set; }
    }
}
