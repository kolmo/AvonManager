using System;
using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Interfaces;

namespace AvonManager.ArtikelModule.Common
{
    internal class SerieFilterEntry : FilterEntryBase,  IFilterEntry
    {
        private SerieDto _serie;
        public SerieFilterEntry(SerieDto serie )
        {
            _serie = serie;
        }
        public string DisplayName
        {
            get
            {
                return _serie.Name;
            }
        }

        public int ID
        {
            get
            {
                return _serie.SerienId;
            }
        }
    }

    internal class NullSerieFilterEntry : IFilterEntry
    {
        public string DisplayName
        {
            get
            {
                return "<Keine Serie>";
            }
        }

        public int ID
        {
            get
            {
                return -1;
            }
        }

        public bool IsSelected
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
