using FavouriteManager.Services.implementation;
using FavouriteManager.Persistence.entity;
using FavouriteManager.Data;
using Microsoft.EntityFrameworkCore;

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
        }
    }
    }
}