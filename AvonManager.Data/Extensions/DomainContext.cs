using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.Data
{
    public partial class AvDBaseEntities
    {
        public IQueryable<Artikel> GetArtikelbySerienAndKategorien(IEnumerable<int> serienIds, IEnumerable<int> katIds, IEnumerable<int> markierungenIds, string artikelNummer, string suchString, bool markierungInverted)
        {
            IQueryable<Artikel> result;
            if (serienIds != null && serienIds.Count() > 0)
            {
                result = this.Artikels.Include("Varianten").Include("Serien").Include("Kategorien").Include("Markierungen").Where(x => x.SerienId.HasValue ? serienIds.Contains(x.SerienId.Value) : false);
            }
            else
            {
                result = this.Artikels.Include("Varianten").Include("Serien").Include("Kategorien").Include("Markierungen");
            }
            if (katIds != null && katIds.Count() > 0)
            {
                result = result.Where(x => x.Kategorien.Any(xx => katIds.Contains(xx.KategorieId)));
            }
            if (markierungenIds != null && markierungenIds.Count() > 0)
            {
                if (markierungInverted)
                {
                    result = result.Where(x => !(x.Markierungen.Any(xx => markierungenIds.Contains(xx.MarkierungId))));
                }
                else
                {
                    result = result.Where(x => x.Markierungen.Any(xx => markierungenIds.Contains(xx.MarkierungId)));
                }
            }
            if (!String.IsNullOrWhiteSpace(suchString))
            {
                result = result.Where(x => x.Artikelname.Contains(suchString) || x.Artikelbeschreibung.Contains(suchString) || x.Varianten.FirstOrDefault(xx => xx.Variante.Contains(suchString)) != null);
            }
            if (!String.IsNullOrWhiteSpace(artikelNummer))
            {
                result = result.Where(x => x.ArtikelNr.StartsWith(artikelNummer) || x.Varianten.FirstOrDefault(xx => xx.ArtikelNr.StartsWith(artikelNummer)) != null);
            }
            //string sql = result.ToTraceString();
            return result;
        }
        public IQueryable<Artikel> GetArtikelPaged(int page, int pagesize)
        {
            return this.Artikels.OrderBy(x=>x.ArtikelId).Skip((page-1)*pagesize).Take(pagesize);
        }
    }
}
