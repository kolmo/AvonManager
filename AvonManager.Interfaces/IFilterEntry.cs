namespace AvonManager.Interfaces
{
    public interface IFilterEntry
    {
        string DisplayName { get; }
        bool IsSelected { get; set; }
        int ID { get; }
    }
}
