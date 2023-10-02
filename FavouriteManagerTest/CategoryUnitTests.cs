using FavouriteManager.DTO;
using FavouriteManager.Data;
using Microsoft.EntityFrameworkCore;
using FavouriteManager.Persistence.entity;
using FavouriteManager.Services.implementation;

namespace FavouriteManagerTest
{
    [TestClass]
    public class CategoryUnitTests
    {
        DbContextOptions<AppDBContext> options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDB")
            .Options;

        [TestMethod]
        public void CreateCategory()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                CreateCategoryRequest cat1 = new CreateCategoryRequest("CategoryA");

                CategoryService categoryService = new CategoryService(context);

                CategoryResponse catResponse = categoryService.Create(cat1);

                Assert.IsNotNull(catResponse);
                Assert.AreEqual(1, catResponse.Id);
                Assert.AreEqual("CategoryA", catResponse.Label);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void GetCategories()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                Category cat1 = new Category(1, "CategoryA");
                Category cat2 = new Category(2, "CategoryB");
                Category cat3 = new Category(3, "CategoryC");

                context.categories.AddRange(cat1, cat2, cat3);
                context.SaveChanges();

                CategoryService categoryService = new CategoryService(context);

                List<CategoryResponse> catResponse = categoryService.Get();

                Assert.IsNotNull(catResponse);
                Assert.AreEqual(1, catResponse[0].Id);
                Assert.AreEqual(2, catResponse[1].Id);
                Assert.AreEqual(3, catResponse[2].Id);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void UpdateCategory()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                Category cat1 = new Category(1, "CategoryA");
                Category cat2 = new Category(2, "CategoryB");
                Category cat3 = new Category(3, "CategoryC");

                context.categories.AddRange(cat1, cat2, cat3);
                context.SaveChanges();

                UpdateFavouriteRequest fav = new UpdateFavouriteRequest(1, "label2", "link2", 2);

                UpdateCategoryRequest cat = new UpdateCategoryRequest(1, "CategoryAA");

                CategoryService categoryService = new CategoryService(context);

                categoryService.Update(cat);

                Category updatedCat = context.categories.Where(cat => cat.Id == 1).FirstOrDefault();

                Assert.AreEqual(1, updatedCat.Id);
                Assert.AreEqual("CategoryAA", updatedCat.Label);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void DeleteCategory()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                Category cat1 = new Category(1, "CategoryA");
                Category cat2 = new Category(2, "CategoryB");
                Category cat3 = new Category(3, "CategoryC");

                context.categories.AddRange(cat1, cat2, cat3);
                context.SaveChanges();

                CategoryService categoryService = new CategoryService(context);

                List<long> ids = new List<long> { 1, 2 };
                categoryService.Delete(ids);
                List<Category> cat = context.categories.ToList();

                Assert.IsNotNull(cat);
                Assert.AreEqual(1, cat.Count);

                context.Database.EnsureDeleted();
            }
        }
    }
}
