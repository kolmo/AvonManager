namespace AvonManager.Interfaces
{
    public interface ICustomerSearchCriteria : ICriteriaBase
    {
        string SearchString { get; set; }
        int? BrochureId { get; set; }
        bool? GetsBrochure { get; set; }
        string Initial { get; set; }
        bool? HasOrders { get; set; }
        bool? ActiveCustomersOnly { get; set; }
        bool? InActiveCustomersOnly { get; set; }
    }
}
