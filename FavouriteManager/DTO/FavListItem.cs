using FavouriteManager.Persistence.entity;

namespace FavouriteManager.DTO
{
    public class FavListItem
    {
        public long Id { get; set; }
        public string Link { get; set; }
        public string Label { get; set; }
        public Category Category { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
