using AvonManager.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvonManager.Data.Helpers;

namespace AvonManager.Data
{
    public class KategorieDataProvider : AvonManager.Interfaces.IKategorieProvider
    {
        private readonly Interfaces.IObjectMapper _mapper;
        public KategorieDataProvider(Interfaces.IObjectMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<List<KategorieDto>> ListAllKategorien()
        {
            Task<List<BusinessObjects.KategorieDto>> task = new Task<List<BusinessObjects.KategorieDto>>(() =>
           {
               using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
               {
                   var query = from b in database.Kategoriens
                               orderby b.Name
                               select b;

                   List<BusinessObjects.KategorieDto> list = new List<BusinessObjects.KategorieDto>();
                   foreach (AvonManager.Data.Kategorien kat in query.ToList())
                   {
                       list.Add(_mapper.Map(kat));
                   }
                   return list;
               };
           });
            task.Start();
            return task;
        }
        public Task<List<ArticleCategoryDto>> ListKategorienByArtikel(int artikelId)
        {
            Task<List<ArticleCategoryDto>> task = new Task<List<ArticleCategoryDto>>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Kategorien_x_Artikels
                                where b.ArtikelId == artikelId
                                select b;

                    List<ArticleCategoryDto> list = new List<ArticleCategoryDto>();
                    foreach (Kategorien_x_Artikel mark in query)
                    {
                        list.Add(_mapper.Map(mark));
                    }
                    return list;
                };
            });
            task.Start();
            return task;
        }
        public Task<KategorieDto> LoadCategoryById(int categoryId)
        {
            Task<KategorieDto> task = new Task<KategorieDto>(() =>
            {
                using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                {
                    var query = from b in database.Kategoriens
                                select b;
                    Kategorien category = query.FirstOrDefault(x => x.KategorieId == categoryId);
                    return _mapper.Map(category);
                }
            });
            task.Start();
            return task;
        }

        public void SaveKategorie(KategorieDto kategorie)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var kat = database.Kategoriens.Single(x => x.KategorieId == kategorie.KategorieId);
                kat.Name = kategorie.Name;
                kat.Bemerkung = kategorie.Bemerkung;
                kat.Logo = kategorie.Logo;
                database.SubmitChanges();
            }
        }

        public void DeleteKategorie(int kategorieId)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Kategorien_x_Artikels
                            where b.KategorieId == kategorieId
                            select b;
                foreach (var detail in query)
                {
                    database.Kategorien_x_Artikels.DeleteOnSubmit(detail);
                }
                var kat = database.Kategoriens.Single(x => x.KategorieId == kategorieId);
                database.Kategoriens.DeleteOnSubmit(kat);
                database.SubmitChanges();
            };
        }

        public int AddKategorie(KategorieDto kategorie)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                Kategorien category = _mapper.Map(kategorie);
                database.Kategoriens.InsertOnSubmit(category);
                database.SubmitChanges();
                return category.KategorieId;
            }
        }
        public void UpdateKategorieArtikel(int artikelId, int kategorieId, bool insert)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var query = from b in database.Kategorien_x_Artikels
                            where b.KategorieId == kategorieId && b.ArtikelId == artikelId
                            select b;
                if (!insert)
                {
                    foreach (var detail in query)
                    {
                        database.Kategorien_x_Artikels.DeleteOnSubmit(detail);
                    }
                }
                else
                {
                    if (!query.Any())
                    {
                        Kategorien_x_Artikel kxa = new Kategorien_x_Artikel { ArtikelId = artikelId, KategorieId = kategorieId };
                        database.Kategorien_x_Artikels.InsertOnSubmit(kxa);
                    }
                }
                try
                {
                    database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Task<Dictionary<int, int>> ListArticleCountsByCategory(int[] categoryIds)
        {
            Task<Dictionary<int, int>> task = new Task<Dictionary<int, int>>(() =>
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();
                if (categoryIds?.Length > 0)
                {
                    using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
                    {
                        var query = from c in database.Kategorien_x_Artikels
                                    where categoryIds.Contains(c.KategorieId)
                                    group c by c.KategorieId into grpCategorys
                                    select new
                                    {
                                        categoryId = grpCategorys.Key,
                                        cnt = grpCategorys.Count()
                                    };

                        foreach (var grp in query)
                        {
                            dic[grp.categoryId] = grp.cnt;
                        }
                    }
                }
                return dic;
            });
            task.Start();
            return task;

        }

        public void AddCategoryArtikel(ArticleCategoryDto articleCategory)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Kategorien_x_Artikels.SingleOrDefault(b => b.KategorieId == articleCategory.CategoryId && b.ArtikelId == articleCategory.ArtikelId);

                if (entity == null)
                {
                    database.Kategorien_x_Artikels.InsertOnSubmit(_mapper.Map(articleCategory));
                }
                try
                {
                    database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void DeleteCategoryArtikel(ArticleCategoryDto articleCategory)
        {
            using (AvonDatabaseDataContext database = new AvonDatabaseDataContext())
            {
                var entity = database.Kategorien_x_Artikels.SingleOrDefault(b => b.KategorieId == articleCategory.CategoryId && b.ArtikelId == articleCategory.ArtikelId);
                if (entity != null)
                {
                    database.Kategorien_x_Artikels.DeleteOnSubmit(entity);
                    database.SubmitChanges();
                }
            }
        }
    }
}
