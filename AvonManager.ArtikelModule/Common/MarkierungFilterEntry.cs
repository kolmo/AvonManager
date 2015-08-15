using System;
using AvonManager.ArtikelModule.Interfaces;

namespace AvonManager.ArtikelModule.Common
{
    internal class MarkierungFilterEntry : FilterEntryBase, IExtendedFilterEntry
    {
        private AvonManager.BusinessObjects.MarkierungDto _markierung;
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
