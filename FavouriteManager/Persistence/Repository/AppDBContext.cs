using Microsoft.EntityFrameworkCore;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options) { }

        public DbSet<Favourite> favourites { get; set; }

        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Favourite>()
                .HasOne(x => x.Category)  // One Favourite has one category
                .WithMany(x => x.Favourites)   // A category can have multiple favourites
                .HasForeignKey(x => x.CategoryId);
        }

        public long Test(int id)
        {
            Category category = new Category(3, "Cat3");

            Favourite favourite1 = new Favourite(3, "Link2", "Label2", true, category, DateTime.Now);

            favourites.Add(favourite1);

            SaveChanges();
            
            Favourite fav = (from x in favourites
                    where x.Id == id
                    select x).FirstOrDefault();

            return fav.CategoryId;
        }
    }
}