using AvonManager.ArtikelModule.Interfaces;

namespace AvonManager.ArtikelModule.Common
{
    internal class KategorieFilterEntry : FilterEntryBase, IFilterEntry
    {
        private AvonManager.BusinessObjects.KategorieDto _kategorie;
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
