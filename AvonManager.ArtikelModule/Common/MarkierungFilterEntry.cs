using AvonManager.Common.Base;
using AvonManager.Interfaces;

namespace AvonManager.ArtikelModule.Common
{
    public class MarkierungFilterEntry : FilterEntryBase, IExtendedFilterEntry
    {
        private AvonManager.BusinessObjects.MarkierungDto _markierung;
        public MarkierungFilterEntry()
        {

        }
        public MarkierungFilterEntry(AvonManager.BusinessObjects.MarkierungDto markierung)
        {
            _markierung = markierung;
        }

        public int? ColorCode
        {
            get
            {
                return _markierung.FarbeARGB;
            }
        }

        public string DisplayName
        {
            get
            {
                return _markierung.Titel;
            }
        }

        public int ID
        {
            get
            {
                return _markierung.MarkierungId;
            }
        }

        public byte[] Symbol
        {
            get
            {
                return null;
            }
        }
    }
}
