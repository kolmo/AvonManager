using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Data.Interfaces
{
    public interface IObjectMapper
    {
        Markierungen_x_Artikel Map(ArtikelMarkierungenDto source);
        Bestellung Map(BestellungDto source);
        BestellungDto Map(Bestellung source);
        BestelldetailDto Map(Bestelldetail source);
        Bestelldetail Map(BestelldetailDto source);
        BestellstatusDto Map(Bestellstatus source);
        HeftDto Map(Hefte source);
        Hefte_x_Kunden Map(HeftKundeDto source);
        KategorieDto Map(Kategorien source);
        KundeDto Map(Kunden source);
        MarkierungDto Map(Markierungen source);
        Serien Map(SerieDto source);
        SerieDto Map(Serien source);
        ArtikelMarkierungenDto Map(Markierungen_x_Artikel source);
        Markierungen Map(MarkierungDto source);
        Kunden Map(KundeDto source);
        Kategorien Map(KategorieDto source);
        HeftKundeDto Map(Hefte_x_Kunden source);
        Hefte Map(HeftDto source);
        Artikel Map(ArtikelDto source);
        ArtikelDto Map(Artikel source);

    }
}
