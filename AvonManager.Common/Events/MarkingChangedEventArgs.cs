namespace AvonManager.Common.Events
{
    public class MarkingChangedEventArgs : ChangedBaseEventArgs
    {
        public BusinessObjects.MarkierungDto Marking { get; set; }
    }
}
