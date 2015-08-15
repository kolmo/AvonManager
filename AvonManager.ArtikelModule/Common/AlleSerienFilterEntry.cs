using AvonManager.ArtikelModule.Interfaces;

namespace AvonManager.ArtikelModule.Common
{
    public class AlleSerienFilterEntry : IFilterEntry
    {
        public string DisplayName
        {
            get
            {
                return "<Alle>";
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
                return false;
            }

            set
            {
                ;
            }
        }
    }
}
