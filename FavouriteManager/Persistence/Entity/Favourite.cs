﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavouriteManager.Persistence.entity
{
    [Table("Favourite")]
    public class Favourite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id {  get; set; }

        [Required]
        [Column]
        public String Link { get; set; }

        [Required]
        [Column]
        public String Label { get; set; }
        [Column]
        public bool IsValid { get; set; }
        
        public Category Category { get; set; }

        [Required]
        [Column]
        public long CategoryId {  get; set; }
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
