using System;
using System.Linq;
using BO = AvonManager.BusinessObjects;

namespace AvonManager.Data.Helpers
{
    public class EntityBOMapper : Interfaces.IObjectMapper
    {
        public BO.ArtikelDto Map(Artikel source)
        {
            BO.ArtikelDto target = new BO.ArtikelDto()
            {
                Beschreibung = source.Artikelbeschreibung,
                Bild = source.Bild?.ToArray(),
                ArtikelId = source.ArtikelId,
                Name = source.Artikelname,
                Nummer = source.ArtikelNr,
                Einzelpreis = source.Einzelpreis,
                Inhalt = source.Inhalt,
                Lagerbestand = source.Lagerbestand,
                SerienId = source.SerienId,
                ChangedAt = source.ChangedAt
            };
            return target;
        }
        public Artikel Map(BO.ArtikelDto source)
        {
            Artikel target = new Artikel()
            {
                Artikelbeschreibung = source.Beschreibung,
                ArtikelId = source.ArtikelId,
                Artikelname = source.Name,
                ArtikelNr = source.Nummer,
                Einzelpreis = source.Einzelpreis,
                Inhalt = source.Inhalt,
                Lagerbestand = source.Lagerbestand,
                SerienId = source.SerienId,
                Bild = source.Bild,
                ChangedAt = source.ChangedAt
            };
            return target;
        }

        public Kategorien Map(BO.KategorieDto source)
        {
            Kategorien target = new Kategorien()
            {
                KategorieId = source.KategorieId,
                Name = source.Name,
                Parent = source.ParentId,
                Reihenfolge = source.Reihenfolge,
                Bemerkung = source.Bemerkung,
                Logo = source.Logo
            };
            return target;
        }
        public BO.KategorieDto Map(Kategorien source)
        {
            BO.KategorieDto target = new BO.KategorieDto()
            {
                KategorieId = source.KategorieId,
                Name = source.Name,
                ParentId = source.Parent,
                Reihenfolge = source.Reihenfolge,
                Bemerkung = source.Bemerkung,
                Logo = source.Logo?.ToArray()
            };
            return target;
        }
        public Markierungen Map(BO.MarkierungDto source)
        {
            Markierungen target = new Markierungen()
            {
                MarkierungId = source.MarkierungId,
                EntitaetId = source.EntitaetId,
                Titel = source.Titel,
                FarbeARGB = source.FarbeARGB,
                Bemerkung = source.Bemerkung
            };
            return target;
        }
        public BO.MarkierungDto Map(Markierungen source)
        {
            BO.MarkierungDto target = new BO.MarkierungDto()
            {
                MarkierungId = source.MarkierungId,
                EntitaetId = source.EntitaetId,
                Titel = source.Titel,
                FarbeARGB = source.FarbeARGB,
                Bemerkung = source.Bemerkung
            };
            return target;
        }
        public BO.ArtikelMarkierungenDto Map(Markierungen_x_Artikel source)
        {
            BO.ArtikelMarkierungenDto target = new BO.ArtikelMarkierungenDto()
            {
                ArtikelId = source.ArtikelId,
                MarkierungId = source.MarkierungId
            };
            return target;
        }
        public Markierungen_x_Artikel Map(BO.ArtikelMarkierungenDto source)
        {
            Markierungen_x_Artikel target = new Markierungen_x_Artikel()
            {
                ArtikelId = source.ArtikelId,
                MarkierungId = source.MarkierungId
            };
            return target;
        }

        public BO.BestelldetailDto Map(Bestelldetail source)
        {
            BO.BestelldetailDto target = new BusinessObjects.BestelldetailDto
            {
                Artikelbeschreibung = source.Artikelbeschr,
                Artikelnummer = source.ArtikelNr,
                BestellId = source.BestellId,
                Campagne = source.Campagne,
                DetailId = source.DetailId,
                Einzelpreis = source.Einzelpreis,
                FDG = source.FGD,
                Jahr = source.Jahr,
                Menge = source.Menge,
                Seite = source.Seite
            };
            return target;
        }

        public BO.BestellstatusDto Map(Bestellstatus source)
        {
            BO.BestellstatusDto target = new BusinessObjects.BestellstatusDto
            {
                Beschreibung = source.Beschreibung,
                Status = source.Status,
                StatusId = source.StatusId
            };
            return target;
        }

        public BO.ArticleCategoryDto Map(Kategorien_x_Artikel source)
        {
            BO.ArticleCategoryDto target = new BO.ArticleCategoryDto()
            {
                ArtikelId = source.ArtikelId,
                CategoryId = source.KategorieId
            };
            return target;
        }

        public Kategorien_x_Artikel Map(BO.ArticleCategoryDto source)
        {
            Kategorien_x_Artikel target = new Kategorien_x_Artikel()
            {
                ArtikelId = source.ArtikelId,
                KategorieId = source.CategoryId
            };
            return target;
        }

        public Bestelldetail Map(BO.BestelldetailDto source)
        {
            Bestelldetail target = new Bestelldetail
            {
                Artikelbeschr = source.Artikelbeschreibung,
                ArtikelNr = source.Artikelnummer,
                BestellId = source.BestellId,
                Campagne = source.Campagne,
                DetailId = source.DetailId,
                Einzelpreis = source.Einzelpreis,
                FGD = source.FDG,
                Jahr = source.Jahr,
                Menge = source.Menge,
                Seite = source.Seite
            };
            return target;
        }

        public BO.SerieDto Map(Serien source)
        {
            BO.SerieDto target = new BO.SerieDto
            {
                Bemerkung = source.Bemerkung,
                Logo = source.Logo?.ToArray(),
                Name = source.Name,
                SerienId = source.SerienId
            };
            return target;
        }
        public Serien Map(BO.SerieDto source)
        {
            Serien target = new Serien
            {
                Bemerkung = source.Bemerkung,
                Logo = source.Logo?.ToArray(),
                Name = source.Name,
                SerienId = source.SerienId
            };
            return target;
        }
        public BO.HeftDto Map(Hefte source)
        {
            BO.HeftDto target = new BusinessObjects.HeftDto
            {
                Beschreibung = source.Beschreibung,
                Bild = source.Bild?.ToArray(),
                HeftId = source.HeftId,
                Jahr = source.Jahr,
                Titel = source.Titel
            };
            return target;
        }
        public Hefte Map(BO.HeftDto source)
        {
            Hefte target = new Hefte
            {
                Beschreibung = source.Beschreibung,
                Bild = source.Bild?.ToArray(),
                HeftId = source.HeftId,
                Jahr = source.Jahr,
                Titel = source.Titel
            };
            return target;
        }
        public BO.KundeDto Map(Kunden source)
        {
            BO.KundeDto target = new BO.KundeDto()
            {
                Adresse = source.Adresse,
                KundenId = source.KundenId,
                Anmerkungen = source.Anmerkungen,
                BekommtHeft = source.BekommtHeft,
                Bild = source.Bild?.ToArray(),
                EmailAdresse = source.EmailAdresse,
                Faxnummer = source.Faxnummer,
                Geburtsdatum = source.Geburtsdatum,
                Inaktiv = source.Inaktiv,
                MobilesTelefon = source.MobilesTelefon,
                Nachname = source.Nachname,
                Ort = source.Ort,
                Postleitzahl = source.Postleitzahl,
                TelefonBeruflich = source.TelefonBeruflich,
                TelefonPrivat = source.TelefonPrivat,
                Vorname = source.Vorname
            };
            return target;
        }
        public Kunden Map(BO.KundeDto source)
        {
            Kunden target = new Kunden()
            {
                Adresse = source.Adresse,
                KundenId = source.KundenId,
                Anmerkungen = source.Anmerkungen,
                BekommtHeft = source.BekommtHeft,
                Bild = source.Bild?.ToArray(),
                EmailAdresse = source.EmailAdresse,
                Faxnummer = source.Faxnummer,
                Geburtsdatum = source.Geburtsdatum,
                Inaktiv = source.Inaktiv,
                MobilesTelefon = source.MobilesTelefon,
                Nachname = source.Nachname,
                Ort = source.Ort,
                Postleitzahl = source.Postleitzahl,
                TelefonBeruflich = source.TelefonBeruflich,
                TelefonPrivat = source.TelefonPrivat,
                Vorname = source.Vorname
            };
            return target;
        }
        public BO.HeftKundeDto Map(Hefte_x_Kunden source)
        {
            BO.HeftKundeDto target = new BO.HeftKundeDto
            {
                HeftId = source.HeftId,
                KundenId = source.KundenId,
                HasOrdered = source.HatBestellt == true,
                ReceivedAt = source.Erhalten
            };
            return target;
        }
        public Hefte_x_Kunden Map(BO.HeftKundeDto source)
        {
            Hefte_x_Kunden target = new Hefte_x_Kunden()
            {
                HeftId = source.HeftId,
                KundenId = source.KundenId,
                HatBestellt = source.HasOrdered,
                Erhalten = source.ReceivedAt
            };
            return target;
        }
        public BO.BestellungDto Map(Bestellung source)
        {
            BO.BestellungDto target = new BO.BestellungDto
            {
                Bemerkung = source.Bemerkung,
                BestellId = source.BestellId,
                Datum = source.Datum,
                KundenId = source.KundenId,
                Loeschvormerkung = source.Loeschvormerkung,
                StatusId = source.StatusId,
                ChangedAt = source.ChangedAt
            };
            return target;
        }
        public Bestellung Map(BO.BestellungDto source)
        {
            Bestellung target = new Bestellung
            {
                Bemerkung = source.Bemerkung,
                BestellId = source.BestellId,
                Datum = source.Datum,
                KundenId = source.KundenId,
                Loeschvormerkung = source.Loeschvormerkung,
                StatusId = source.StatusId,
                ChangedAt = source.ChangedAt
            };
            return target;
        }
    }
}
