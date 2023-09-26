using FavouriteManager.Services.implementation;
using FavouriteManager.Persistence.entity;
using FavouriteManager.Data;
using Microsoft.EntityFrameworkCore;
using FavouriteManager.DTO;

namespace FavouriteManagerTest
{
    [TestClass]
    public class FavouriteUnitTests
    {
        DbContextOptions<AppDBContext> options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDB")
            .Options;

        static Category category1 = new Category(1, "Cat1");

        static DateTime lastUpdated = DateTime.Now;

        Favourite favourite1 = new Favourite(1, "Link1", "Label1", true, category1, lastUpdated);

        [TestMethod]
        public void TestCategory()
        {
            Assert.AreEqual(category1.Id, 1);
            Assert.AreEqual(category1.Label,  "Cat1");

            Assert.AreEqual(favourite1.Id, 1);
            Assert.AreEqual(favourite1.Link, "Link1");
            Assert.AreEqual(favourite1.Label, "Label1");
            Assert.AreEqual(favourite1.IsValid, true);
            Assert.AreEqual(favourite1.Category, category1);
            Assert.AreEqual(favourite1.UpdatedAt, lastUpdated);
        }
    

    [TestMethod]
    public void FilterByCategory_Should_Return_Filtered_Favoris()
    {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
        {
                Category cat1 = new Category(1, "CategoryA");
                Category cat2 = new Category(2, "CategoryB");
                context.favourites.AddRange(
                
                new List<Favourite>
            {
                new Favourite { Id = 1, Link = "link1", Label = "link1", Category = cat2 },
                new Favourite { Id = 2, Link = "link1", Label = "link2", Category = cat1 },
                new Favourite { Id = 3, Link = "link1", Label = "link3", Category = cat1 },
            });

            context.SaveChanges();

            var favouriteService = new FavouriteService(context);

            // Act
            var filteredFavourites = favouriteService.FilterByCategory(1);

            // Assert
            Assert.IsNotNull(filteredFavourites);
            Assert.AreEqual(2, filteredFavourites.Count);
            context.Database.EnsureDeleted();
            }
    }
        [TestMethod]
        public void CreateFavourite()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                Category cat1 = new Category(1, "CategoryA");
                context.categories.Add(cat1);
                context.SaveChanges();

                CreateFavouriteRequest fav1 = new CreateFavouriteRequest("label1", "link1", 1);

                FavouriteService favouriteService = new FavouriteService(context);

                FavouriteResponse favResponse = favouriteService.Create(fav1);

                Assert.IsNotNull(favResponse);
                Assert.AreEqual(1, favResponse.Id);
                Assert.AreEqual("CategoryA", favResponse.Category.Label);
                context.Database.EnsureDeleted();
            }

        }
        [TestMethod]
        public void UpdateFavourite()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {

                Category cat1 = new Category(1, "CategoryA");
                Category cat2 = new Category(2, "CategoryB");
                Favourite fav1 = new Favourite { Id = 1, Link = "link1", Label = "link1", Category = cat1 };
                context.categories.AddRange(cat1, cat2);
                context.favourites.Add(fav1);
                context.SaveChanges();

                UpdateFavouriteRequest fav = new UpdateFavouriteRequest(1, "label2", "link2", 2);

                FavouriteService favouriteService = new FavouriteService(context);

                favouriteService.Update(fav);
                Favourite updatedFav = context.favourites.Where(fav => fav.Id == 1).FirstOrDefault();
                Assert.AreEqual(1, updatedFav.Id);
                Assert.AreEqual(2, updatedFav.Category.Id);
                Assert.AreEqual("link2", updatedFav.Link);
                Assert.AreEqual("label2", updatedFav.Label);
                context.Database.EnsureDeleted();
            }
        }
        [TestMethod]
        public void GetFavourites()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {

                Category cat1 = new Category(1, "CategoryA");
                Favourite fav1 = new Favourite { Id = 1, Link = "link1", Label = "link1", Category = cat1 };
                Favourite fav2 = new Favourite { Id = 2, Link = "link2", Label = "link2", Category = cat1 };
                context.categories.Add(cat1);
                context.favourites.AddRange(fav1, fav2);
                context.SaveChanges();

                FavouriteService favouriteService = new FavouriteService(context);

                List<FavouriteResponse> favResponse = favouriteService.Get();
                Assert.IsNotNull(favResponse);
                Assert.AreEqual(1, favResponse[0].Id);
                Assert.AreEqual(2, favResponse[1].Id);
                Assert.AreEqual(1, favResponse[1].Category.Id);
                context.Database.EnsureDeleted();
            }
        }
        
        [TestMethod]
        public void DeleteFavourite()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {

                Category cat1 = new Category(1, "CategoryA");
                Favourite fav1 = new Favourite { Id = 1, Link = "link1", Label = "link1", Category = cat1 };
                Favourite fav2 = new Favourite { Id = 2, Link = "link2", Label = "link2", Category = cat1 };
                context.categories.Add(cat1);
                context.favourites.AddRange(fav1, fav2);
                context.SaveChanges();

                FavouriteService favouriteService = new FavouriteService(context);

                List<long> ids = new List<long> { 1 };
                favouriteService.Delete(ids);
                List<Favourite> fav = context.favourites.ToList();
                Assert.IsNotNull(fav);
                Assert.AreEqual(1, fav.Count);
                context.Database.EnsureDeleted();
            }
        }
        
    }
}