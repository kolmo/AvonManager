namespace AvonManager.Common.Events
{
    public class BrochureChangedEventArgs : ChangedBaseEventArgs
    {
        public BusinessObjects.HeftDto Brochure { get; set; }
    }
}
