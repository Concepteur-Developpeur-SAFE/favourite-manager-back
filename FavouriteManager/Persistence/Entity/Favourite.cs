using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavouriteManager.Persistence.entity
{
    /// <summary>
    /// This class represents a Favorite entity
    /// </summary>
    [Table("Favourite")]
    public class Favourite
    {
        /// <summary>
        /// Gets or sets the unique id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id {  get; set; }

        /// <summary>
        /// Gets or sets the required link
        /// </summary>
        [Required]
        [Column]
        public String Link { get; set; }

        /// <summary>
        /// Gets or sets the required label
        /// </summary>
        [Required]
        [Column]
        public String Label { get; set; }

        /// <summary>
        /// Gets or sets if the favorites link is valid or not
        /// </summary>
        [Column]
        public bool IsValid { get; set; }
        
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the category id to which the Favorite entity belongs
        /// </summary>
        [Required]
        [Column]
        public long CategoryId {  get; set; }

        /// <summary>
        /// Gets or sets the latest update
        /// </summary>
        [Column]
        public DateTime UpdatedAt { get; set; }


        public Favourite(long Id, String Link, String Label, bool IsValid, Category Category, DateTime UpdatedAt)
        {
            this.Id = Id;
            this.Link = Link;
            this.Label = Label;
            this.IsValid = IsValid;
            this.Category = Category;
            this.UpdatedAt = UpdatedAt;
        }

        public Favourite() { }
    }
}
