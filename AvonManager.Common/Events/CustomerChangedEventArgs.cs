namespace AvonManager.Common.Events
{
    public class CustomerChangedEventArgs : ChangedBaseEventArgs
    {
        public BusinessObjects.KundeDto Customer { get; set; }
    }
}
