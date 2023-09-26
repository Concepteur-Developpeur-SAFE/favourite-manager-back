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
            Assert.AreEqual(category1.Label, "Cat1");

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

                //delete the in-memory database after each test
                context.Database.EnsureDeleted();
            }

        }

        [TestMethod]
        public void SortByCategory_Should_Return_Sorted_Favoris()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                //recreate the database in memory before each test
                context.Database.EnsureCreated();
                Category catA = new Category(1, "dev");
                Category catB = new Category(2, "agility");
                context.favourites.AddRange(

                new List<Favourite>
            {
                new Favourite { Id = 1, Link = "link1", Label = "link1", Category = catA },
                new Favourite { Id = 2, Link = "link2", Label = "link2", Category = catB },
                new Favourite { Id = 3, Link = "link3", Label = "link3", Category = catA },
                new Favourite { Id = 4, Link = "link4", Label = "link4", Category = catB },
            });

                context.SaveChanges();

                var favouriteService = new FavouriteService(context);

                // Act
                var sortedFavourites = favouriteService.SortByCategory();

                // Assert
                Assert.IsTrue(IsSortedByCategory(sortedFavourites));

                //delete the in-memory database after each test
                context.Database.EnsureDeleted();
            }
        }
        private bool IsSortedByCategory(List<Favourite> favourites)
        {
            for (int i = 0; i < favourites.Count - 1; i++)
            {
                if (string.Compare(favourites[i].Category.Label, favourites[i + 1].Category.Label) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        [TestMethod]
        public void SortByDate_Should_Return_Sorted_Favoris()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                //recreate the database in memory before each test
                context.Database.EnsureCreated();
                Category catA = new Category(1, "dev");
                Category catB = new Category(2, "agility");
                context.favourites.AddRange(

                new List<Favourite>
            {
                new Favourite { Id = 1, Link = "link1", Label = "link1", Category = catA, UpdatedAt = DateTime.Now.AddHours(-2) },
                new Favourite { Id = 2, Link = "link2", Label = "link2", Category = catB, UpdatedAt = DateTime.Now.AddHours(-1) },
                new Favourite { Id = 3, Link = "link3", Label = "link3", Category = catA, UpdatedAt = DateTime.Now.AddHours(-4)},
                new Favourite { Id = 4, Link = "link4", Label = "link4", Category = catB, UpdatedAt = DateTime.Now.AddHours(-3) },
            });

                context.SaveChanges();

                var favouriteService = new FavouriteService(context);

                // Act
                var sortedFavourites = favouriteService.SortByDate();

                // Assert
                Assert.IsTrue(IsSortedByDate(sortedFavourites));

                //delete the in-memory database after each test
                context.Database.EnsureDeleted();
            }
        }
        private bool IsSortedByDate(List<Favourite> favourites)
        {
            for (int i = 0; i < favourites.Count - 1; i++)
            {
                if (favourites[i].UpdatedAt > favourites[i + 1].UpdatedAt)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
