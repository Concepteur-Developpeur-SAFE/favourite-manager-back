using FavouriteManager.DTO;
using FavouriteManager.Data;
using Microsoft.EntityFrameworkCore;
using FavouriteManager.Persistence.entity;
using FavouriteManager.Services.implementation;

namespace FavouriteManagerTest
{
    [TestClass]
    public class FavouriteUnitTests
    {
        DbContextOptions<AppDBContext> options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDB")
            .Options;
        
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

                // Assert
                Assert.IsNotNull(filteredFavourites);
                Assert.AreEqual(2, filteredFavourites.Count);

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

        private bool IsSortedByCategory(List<FavouriteResponse> favourites)
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

        private bool IsSortedByDate(List<FavouriteResponse> favourites)
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

        [TestMethod]
        public async Task CheckLinksAsync_Should_MarkBrokenLinks()
        {
            // Arrange
            using (var context = new AppDBContext(options))
            {
                context.Database.EnsureCreated();
                Category catA = new Category(1, "dev");
                Category catB = new Category(2, "agility");
                context.favourites.AddRange(

                new List<Favourite>
            {
                new Favourite { Id = 1, Link = "https://example.com/nonexistent", Label = "link1", Category = catA, UpdatedAt = DateTime.Now.AddHours(-2) },
                new Favourite { Id = 2, Link = "https://www.google.fr", Label = "link3", Category = catB, UpdatedAt = DateTime.Now.AddHours(-1) },
            });

                context.SaveChanges();
                var httpClientHandler = new HttpClientHandler();

                var httpClient = new HttpClient(httpClientHandler);
                var favouriteService = new FavouriteService(context);
                var favourites = await context.favourites.ToListAsync();

                // Act
                await favouriteService.CheckLinksAsync(favourites[0]);
                await favouriteService.CheckLinksAsync(favourites[1]);

                // Assert
                Assert.IsFalse(favourites[0].IsValid);
                Assert.IsTrue(favourites[1].IsValid);

                //delete the in-memory database after each test
                context.Database.EnsureDeleted();
            }
        }
    }
}