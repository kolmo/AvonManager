using AvonManager.Common.Base;
using AvonManager.Interfaces;

namespace AvonManager.ArtikelModule.Common
{
    public class KategorieFilterEntry : FilterEntryBase, IFilterEntry
    {
        private AvonManager.BusinessObjects.KategorieDto _kategorie;
        public KategorieFilterEntry()
        {

        }
        public KategorieFilterEntry(AvonManager.BusinessObjects.KategorieDto kategorie)
        {
            _kategorie = kategorie;
        }
        public string DisplayName
        {
            get
            {
                return _kategorie.Name;
            }
        }

        public int ID
        {
            get
            {
                return _kategorie.KategorieId;
            }
        }    
    }
}
