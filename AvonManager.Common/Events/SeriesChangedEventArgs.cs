namespace AvonManager.Common.Events
{
    public class SeriesChangedEventArgs : ChangedBaseEventArgs
    {
        public BusinessObjects.SerieDto Series { get; set; }
    }
}
