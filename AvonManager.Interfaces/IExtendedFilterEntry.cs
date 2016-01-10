namespace AvonManager.Interfaces
{
    public interface IExtendedFilterEntry : IFilterEntry
    {
        byte[] Symbol { get; }
        int? ColorCode { get; }
    }
}
