using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = AvonManager.BusinessObjects;

namespace AvonManager.Data.Helpers
{
    public class EntityBOMapper : Interfaces.IObjectMapper
    {
        public Target Map<Target>(object src) where Target : class
        {
            if (src is Artikel)
            {
                Artikel source = src as Artikel;
                BO.Artikel target = new BO.Artikel()
                {
                    Beschreibung = source.Artikelbeschreibung,
                    ArtikelId = source.ArtikelId,
                    Name = source.Artikelname,
                    Nummer = source.ArtikelNr,
                    Einzelpreis = source.Einzelpreis,
                    Inhalt = source.Inhalt,
                    Lagerbestand = source.Lagerbestand,
                    SerienId = source.SerienId
                };
                return target as Target;
            }
            else if (src is BO.Artikel)
            {
                BO.Artikel source = src as BO.Artikel;
                Artikel target = new Artikel()
                {
                    Artikelbeschreibung = source.Beschreibung,
                    ArtikelId = source.ArtikelId,
                    Artikelname = source.Name,
                    ArtikelNr = source.Nummer,
                    Einzelpreis = source.Einzelpreis,
                    Inhalt = source.Inhalt,
                    Lagerbestand = source.Lagerbestand,
                    SerienId = source.SerienId
                };
                return target as Target;
            }
            else if (src is ArtikelVarianten)
            {
                ArtikelVarianten source = src as ArtikelVarianten;
                BO.ArtikelVariante target = new BusinessObjects.ArtikelVariante()
                {
                    ArtikelId = source.ArtikelId,
                    Artikelnummer = source.ArtikelNr,
                    Einzelpreis = source.Einzelpreis,
                    Lagerbestand = source.Lagerbestand,
                    Variante = source.Variante,
                    VariantenId = source.VariantenId
                };
                return target as Target;
            }
            else if (src is BO.ArtikelVariante)
            {
                BO.ArtikelVariante source = src as BO.ArtikelVariante;
                ArtikelVarianten target = new ArtikelVarianten()
                {
                    ArtikelId = source.ArtikelId,
                    ArtikelNr = source.Artikelnummer,
                    Einzelpreis = source.Einzelpreis,
                    Lagerbestand = source.Lagerbestand,
                    Variante = source.Variante,
                    VariantenId = source.VariantenId
                };
                return target as Target;
            }
            else if (src is BO.KategorieDto)
            {
                BO.KategorieDto source = src as BO.KategorieDto;
                Kategorien target = new Kategorien()
                {
                    KategorieId = source.KategorieId,
                    Name = source.Name,
                    Parent = source.ParentId,
                    Reihenfolge = source.Reihenfolge,
                    Bemerkung = source.Bemerkung,
                    Logo = source.Logo
                };
                return target as Target;
            }
            else if (src is Kategorien)
            {
                Kategorien source = src as Kategorien;
                BO.KategorieDto target = new BO.KategorieDto()
                {
                    KategorieId = source.KategorieId,
                    Name = source.Name,
                    ParentId = source.Parent,
                    Reihenfolge = source.Reihenfolge,
                    Bemerkung = source.Bemerkung,
                    Logo = source.Logo != null ? source.Logo.ToArray() : null
                };
                return target as Target;
            }
            else if (src is BO.MarkierungDto)
            {
                BO.MarkierungDto source = src as BO.MarkierungDto;
                Markierungen target = new Markierungen()
                {
                    MarkierungId = source.MarkierungId,
                    EntitaetId = source.EntitaetId,
                    Titel = source.Titel,
                    FarbeARGB = source.FarbeARGB,
                    Bemerkung = source.Bemerkung
                };
                return target as Target;
            }
            else if (src is Markierungen)
            {
                Markierungen source = src as Markierungen;
                BO.MarkierungDto target = new BO.MarkierungDto()
                {
                    MarkierungId = source.MarkierungId,
                    EntitaetId = source.EntitaetId,
                    Titel = source.Titel,
                    FarbeARGB = source.FarbeARGB,
                    Bemerkung = source.Bemerkung
                };
                return target as Target;
            }
            else if (src is Markierungen_x_Artikel)
            {
                Markierungen_x_Artikel source = src as Markierungen_x_Artikel;
                BO.ArtikelMarkierungenDto target = new BusinessObjects.ArtikelMarkierungenDto()
                {
                    ArtikelId = source.ArtikelId,
                    MarkierungId = source.MarkierungId
                };
                return target as Target;
            }
            else if (src is Serien)
            {
                Serien source = src as Serien;
                BO.SerieDto target = new BO.SerieDto
                {
                    Bemerkung= source.Bemerkung,
                    Logo=source.Logo?.ToArray(),
                    Name=source.Name,
                    SerienId=source.SerienId
                };
                return target as Target;
            }
            else
                return null;
        }
    }
}
