using AvonManager.BusinessObjects;

namespace AvonManager.Common.Events
{
    public class CategoryChangedEventArgs : ChangedBaseEventArgs
    {
        public KategorieDto Category { get; set; }
    }
}
