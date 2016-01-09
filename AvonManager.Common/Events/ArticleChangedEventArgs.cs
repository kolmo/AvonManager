using AvonManager.BusinessObjects;

namespace AvonManager.Common.Events
{
    public class ArticleChangedEventArgs : ChangedBaseEventArgs
    {
        public ArtikelDto Article { get; set; }
    }
}
