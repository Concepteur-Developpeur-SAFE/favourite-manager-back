using FavouriteManager.Services.implementation;
using FavouriteManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FavouriteManager.DTO;
using FavouriteManager.Persistence.entity;
using Microsoft.Extensions.Options;
using FavouriteManager.Data;
using Microsoft.EntityFrameworkCore;

namespace FavouriteManagerTest
{
    [TestClass]
    public class IntegrationTest
    {
        DbContextOptions<AppDBContext> options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDB")
            .Options;

        [TestMethod]
        public void CreateFavoriteAndCategory_CheckInteraction()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                //Create 2 new category
                CreateCategoryRequest cat1 = new CreateCategoryRequest("CategoryA");
                CreateCategoryRequest cat2 = new CreateCategoryRequest("CategoryB");
                
                CategoryService categoryService = new CategoryService(context);
                CategoryResponse category1 = categoryService.Create(cat1);
                CategoryResponse category2 = categoryService.Create(cat2);

                //Create 3 new favorite
                CreateFavouriteRequest fav1 = new CreateFavouriteRequest("label1", "link1", 1);
                CreateFavouriteRequest fav2 = new CreateFavouriteRequest("label2", "link2", 2);
                CreateFavouriteRequest fav3 = new CreateFavouriteRequest("label3", "link3", 1);
                
                FavouriteService favouriteService = new FavouriteService(context);
                FavouriteResponse favorite1 = favouriteService.Create(fav1);
                FavouriteResponse favorite2 = favouriteService.Create(fav2);
                FavouriteResponse favorite3 = favouriteService.Create(fav3);

                context.SaveChanges();

                // Act
                var allCategories = categoryService.Get();
                var allFavorites = favouriteService.Get();

                //Assert
                //Verify that the category has been created
                CollectionAssert.Contains(allCategories, category1);
                CollectionAssert.Contains(allCategories, category2);

                //Verify that the favorite has been created and belongs to the category.
                CollectionAssert.Contains(allFavorites, favorite1);
                CollectionAssert.Contains(allFavorites, favorite2);
                CollectionAssert.Contains(allFavorites, favorite3);
                Assert.AreEqual(category1.Id, fav1.CategoryId);
                Assert.AreEqual(category2.Id, fav2.CategoryId);
                Assert.AreEqual(category1.Id, fav3.CategoryId);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void FilterFavoritesByCategory_Check()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                //Create 2 new category
                CreateCategoryRequest cat1 = new CreateCategoryRequest("CategoryA");
                CreateCategoryRequest cat2 = new CreateCategoryRequest("CategoryB");

                CategoryService categoryService = new CategoryService(context);
                CategoryResponse category1 = categoryService.Create(cat1);
                CategoryResponse category2 = categoryService.Create(cat2);

                //Create 3 new favorite
                CreateFavouriteRequest fav1 = new CreateFavouriteRequest("label1", "link1", 1);
                CreateFavouriteRequest fav2 = new CreateFavouriteRequest("label2", "link2", 2);
                CreateFavouriteRequest fav3 = new CreateFavouriteRequest("label3", "link3", 1);

                FavouriteService favouriteService = new FavouriteService(context);
                FavouriteResponse favorite1 = favouriteService.Create(fav1);
                FavouriteResponse favorite2 = favouriteService.Create(fav2);
                FavouriteResponse favorite3 = favouriteService.Create(fav3);

                context.SaveChanges();

                // Act
                var favoritesInCategory1 = favouriteService.FilterByCategory(category1.Id);
                var favoritesInCategory2 = favouriteService.FilterByCategory(category2.Id);

                // Assert
                // Verify that category filtering is working correctly
                CollectionAssert.Contains(favoritesInCategory1, favorite1);
                CollectionAssert.Contains(favoritesInCategory1, favorite3);
                CollectionAssert.DoesNotContain(favoritesInCategory1, favorite2);
                CollectionAssert.Contains(favoritesInCategory2, favorite2);
                CollectionAssert.DoesNotContain(favoritesInCategory2, favorite1);

                context.Database.EnsureDeleted();
            }
            }

        [TestMethod]
        public void SortFavoritesByCategory_Check()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                //Create 2 new category
                CreateCategoryRequest cat1 = new CreateCategoryRequest("CategoryA");
                CreateCategoryRequest cat2 = new CreateCategoryRequest("CategoryB");
                CreateCategoryRequest cat3 = new CreateCategoryRequest("CategoryC");

                CategoryService categoryService = new CategoryService(context);
                CategoryResponse category1 = categoryService.Create(cat1);
                CategoryResponse category2 = categoryService.Create(cat2);
                CategoryResponse category3 = categoryService.Create(cat3);

                //Create 3 new favorite
                CreateFavouriteRequest fav1 = new CreateFavouriteRequest("label1", "link1", 2);
                CreateFavouriteRequest fav2 = new CreateFavouriteRequest("label2", "link2", 3);
                CreateFavouriteRequest fav3 = new CreateFavouriteRequest("label3", "link3", 1);

                FavouriteService favouriteService = new FavouriteService(context);
                FavouriteResponse favorite1 = favouriteService.Create(fav1);
                FavouriteResponse favorite2 = favouriteService.Create(fav2);
                FavouriteResponse favorite3 = favouriteService.Create(fav3);

                context.SaveChanges();

                // Act
                List<FavouriteResponse> sortedFavorites = favouriteService.SortByCategory();
                List<FavouriteResponse> sortedFavoritesDesc = favouriteService.SortByCategoryDesc();

                // Assert
                // Verify that sorting by name works correctly.
                CollectionAssert.AreEqual(sortedFavorites, new List<FavouriteResponse> { favorite3, favorite1, favorite2 });
                CollectionAssert.AreEqual(sortedFavoritesDesc, new List<FavouriteResponse> { favorite2, favorite1, favorite3 });

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void DeleteFavorites_Check()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                //Create 2 new category
                CreateCategoryRequest cat1 = new CreateCategoryRequest("CategoryA");
                CreateCategoryRequest cat2 = new CreateCategoryRequest("CategoryB");

                CategoryService categoryService = new CategoryService(context);
                CategoryResponse category1 = categoryService.Create(cat1);
                CategoryResponse category2 = categoryService.Create(cat2);

                //Create 3 new favorite
                CreateFavouriteRequest fav1 = new CreateFavouriteRequest("label1", "link1", 1);
                CreateFavouriteRequest fav2 = new CreateFavouriteRequest("label2", "link2", 2);
                CreateFavouriteRequest fav3 = new CreateFavouriteRequest("label3", "link3", 1);

                FavouriteService favouriteService = new FavouriteService(context);
                FavouriteResponse favorite1 = favouriteService.Create(fav1);
                FavouriteResponse favorite2 = favouriteService.Create(fav2);
                FavouriteResponse favorite3 = favouriteService.Create(fav3);

                context.SaveChanges();
                
                // Act
                List<long> ids = new List<long> { 1 };
                categoryService.Delete(ids);

                // Assert
                var remainedCategories = categoryService.Get();
                CollectionAssert.DoesNotContain(remainedCategories, category1);

                // Act
                List<long> idsfav = new List<long> { 3 };
                favouriteService.Delete(idsfav);

                // Assert
                var remainedFavorites = favouriteService.Get();
                CollectionAssert.DoesNotContain(remainedFavorites, favorite3);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void SortFavoritesByDate_Check()
        {
            //add mock data to in-memory database using context.Favourites.AddRange
            using (var context = new AppDBContext(options))
            {
                context.Database.EnsureCreated();
                Category catA = new Category(1, "dev");
                Category catB = new Category(2, "agility");


                List<Favourite> favorites = new List<Favourite>();

                Favourite fav1 = new Favourite { Id = 1, Link = "link1", Label = "link1", Category = catA, UpdatedAt = DateTime.Now };
                Favourite fav2 = new Favourite { Id = 2, Link = "link2", Label = "link2", Category = catB, UpdatedAt = DateTime.Now.AddHours(-1) };
                Favourite fav3 = new Favourite { Id = 3, Link = "link3", Label = "link3", Category = catA, UpdatedAt = DateTime.Now.AddHours(1) };
            
                favorites.Add(fav1); 
                favorites.Add(fav2);
                favorites.Add(fav3);

                context.favourites.AddRange(favorites);

                context.SaveChanges();

                var favouriteService = new FavouriteService(context);

                // Act
                List<FavouriteResponse> sortedFavourites = favouriteService.SortByDate();
                List<FavouriteResponse> sortedFavouritesDesc = favouriteService.SortByDateDesc();

                // Assert
                Assert.AreEqual(fav2.Id, sortedFavourites[0].Id);
                Assert.AreEqual(fav1.Id, sortedFavourites[1].Id);
                Assert.AreEqual(fav3.Id, sortedFavourites[2].Id);

                Assert.AreEqual(fav3.Id, sortedFavouritesDesc[0].Id);
                Assert.AreEqual(fav1.Id, sortedFavouritesDesc[1].Id);
                Assert.AreEqual(fav2.Id, sortedFavouritesDesc[2].Id);


                //delete the in-memory database after each test
                context.Database.EnsureDeleted();
            }
        }

    }
}
