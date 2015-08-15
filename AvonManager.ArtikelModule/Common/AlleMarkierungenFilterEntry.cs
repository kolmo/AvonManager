namespace AvonManager.ArtikelModule.Common
{
    public class AlleMarkierungenFilterEntry : Interfaces.IExtendedFilterEntry
    {
        public int? ColorCode
        {
            get
            {
                return default(int?);
            }
        }

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
                return -1 ;
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

        public byte[] Symbol
        {
            get
            {
                return null;
            }
        }
    }
}
