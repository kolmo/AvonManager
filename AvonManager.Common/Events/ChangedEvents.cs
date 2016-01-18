using Microsoft.Practices.Prism.PubSubEvents;

namespace AvonManager.Common.Events
{
    public class ArticleChangedEvent : PubSubEvent<ArticleChangedEventArgs> { }
    public class CategoryChangedEvent : PubSubEvent<CategoryChangedEventArgs> { }
    public class SeriesChangedEvent : PubSubEvent<SeriesChangedEventArgs> { }
    public class MarkingChangedEvent : PubSubEvent<MarkingChangedEventArgs> { }
    public class BrochureChangedEvent : PubSubEvent<BrochureChangedEventArgs> { }
    public class CustomerChangedEvent : PubSubEvent<CustomerChangedEventArgs> { }
    public class ModuleChangedEvent : PubSubEvent<ModuleChangedEventArgs> { }
}
