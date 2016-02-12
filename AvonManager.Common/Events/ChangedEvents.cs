using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.Common.Events
{
    public class ArticleChangedEvent : PubSubEvent<ArticleChangedEventArgs> { }
    public class BrochureChangedEvent : PubSubEvent<BrochureChangedEventArgs> { }
    public class CategoryChangedEvent : PubSubEvent<CategoryChangedEventArgs> { }
    public class CustomerChangedEvent : PubSubEvent<CustomerChangedEventArgs> { }
    public class MarkingChangedEvent : PubSubEvent<MarkingChangedEventArgs> { }
    public class ModuleChangedEvent : PubSubEvent<ModuleChangedEventArgs> { }
    public class OrderChangedEvent : PubSubEvent<OrderChangedEventArgs> { }
    public class SeriesChangedEvent : PubSubEvent<SeriesChangedEventArgs> { }
}
