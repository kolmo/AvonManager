using System;
using System.Windows.Markup;

namespace AvonManager.Helpers
{
    public class MessageStringMarkupExtension : MarkupExtension
    {
        public string Key { get; set; }
        public MessageStringMarkupExtension()
        {
        }
        #region IMarkupExtension<string> Member

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
           return !String.IsNullOrWhiteSpace(Key) ? AvonManager.Desktop.Assets.Messages.ResourceManager.GetString(Key) : string.Empty;
        }

        #endregion
    }
}
