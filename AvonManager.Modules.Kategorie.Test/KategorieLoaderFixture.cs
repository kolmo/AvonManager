using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvonManager.Modules.Kategorie.Test
{
    [TestClass]
    public class KategorieLoaderFixture
    {
        [TestMethod]
        public void EntityLoader()
        {
            //using (AvonManager.Data.AvDBaseEntities db = new Data.AvDBaseEntities())
            //{
            //    Assert.IsNotNull(db.Database.Connection);
            //    var query = from d in db.Artikels
            //                where d.Markierungen.Any(x=>x.MarkierungId == 2) && d.ArtikelId == 47
            //                select d;

            //    Data.Artikel mark = query.FirstOrDefault();
            //    Assert.IsNotNull(mark);
            //    Data.Markierungen m = mark.Markierungen.First(x => x.MarkierungId == 2);
            //    mark.Markierungen.Remove(m);
            //    db.SaveChanges();
            //}
        }
    }
}
