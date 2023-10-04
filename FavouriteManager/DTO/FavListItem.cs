using FavouriteManager.Persistence.entity;

namespace FavouriteManager.DTO
{
    /// <summary>
    /// Represents a data transfer object (DTO) for favorite.
    /// </summary>
    public class FavListItem
    {
        /// <summary>
        /// Gets or sets the favorite ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the favorite Link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the favorite Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the favorite category.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the favorite latest update.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
