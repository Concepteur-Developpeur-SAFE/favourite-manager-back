using FavouriteManager.Persistence.entity;

namespace FavouriteManager.DTO
{
    public record CreateFavouriteRequest(
        string Label,
        string Link,
        long CategoryId
    );
    public record FavouriteResponse(
        long Id,
        string Label,
        string Link,
        CategoryResponse Category,
        DateTime UpdatedAt

    );
    public record CategoryResponse(
        long Id,
        string Label

    );
    public record UpdateFavouriteRequest(
        long Id,
        string Label,
        string Link,
        long CategoryId
    );
}
