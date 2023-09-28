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


    }
}