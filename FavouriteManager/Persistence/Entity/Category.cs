using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavouriteManager.Persistence.entity
{
    /// <summary>
    /// This class represents a Category entity
    /// </summary>
    [Table("Category")]
    public class Category
    {
        /// <summary>
        /// Gets or sets the unique id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the required label
        /// </summary>
        [Required]
        [Column]
        public String Label { get; set; }

        public ICollection<Favourite> Favourites { get; }

        public Category() { }

        public Category(long Id, String Label)
        {
            this.Id = Id;
            this.Label = Label;
        }
    }
}
