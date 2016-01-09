using System;

namespace AvonManager.Common.Events
{
    public enum ChangedType
    {
        None = 0,
        Create,
        Update,
        Delete
    }
    public class ChangedBaseEventArgs : EventArgs
    {
        public ChangedType ChangedType { get; set; }
    }
}
