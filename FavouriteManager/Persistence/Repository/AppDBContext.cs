using Microsoft.EntityFrameworkCore;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Data
{
    /// <summary>
    /// Database context for the application, managing favorites and categories entities.
    /// </summary>
    public class AppDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the AppDBContext database context.
        /// </summary>
        /// <param name="options">Context configuration options.</param>
        public AppDBContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Gets or sets a set of favorite entities.
        /// </summary>
        public DbSet<Favourite> favourites { get; set; }

        /// <summary>
        /// Gets or sets a set of category entities.
        /// </summary>
        public DbSet<Category> categories { get; set; }

        /// <summary>
        /// Configures relationships between entities in the database model.
        /// </summary>
        /// <param name="modelBuilder">The database model builder.</param>
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