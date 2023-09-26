using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavouriteManager.Persistence.entity
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column]
        public String Label { get; set; }

        public ICollection<Favourite> Favourites { get; }

        public Category()
        {

        }

        public Category(long Id, String Label)
        {
            this.Id = Id;
            this.Label = Label;
        }
    }
}
