using AvonManager.ArtikelModule.Interfaces;
using AvonManager.BusinessObjects;

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
}
