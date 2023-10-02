/// <summary>
/// Namespace containing the DTOs (Data Transfer Objects) for managing favorites.
/// </summary>
namespace FavouriteManager.DTO
{
    /// <summary>
    /// Represents a request to create a favorite.
    /// </summary>
    public record CreateFavouriteRequest(
        /// <summary>
        /// Gets or sets the favorite label.
        /// </summary>
        string Label,

        /// <summary>
        /// Gets or sets the favorite link.
        /// </summary>
        string Link,

        /// <summary>
        /// Gets or sets the category id associated with the favorite.
        /// </summary>
        long CategoryId
    );

    /// <summary>
    /// Represents the response of a favorite.
    /// </summary>
    public record FavouriteResponse(
        /// <summary>
        /// Gets the favorite's id.
        /// </summary>
        long Id,

        /// <summary>
        /// Gets the label of the favorite.
        /// </summary>
        string Label,

        /// <summary>
        /// Gets the favorite link.
        /// </summary>
        string Link,

        /// <summary>
        /// Gets the category associated with the favorite.
        /// </summary>
        CategoryResponse Category,

        /// <summary>
        /// Gets the lasted update date of the favorite.
        /// </summary>
        DateTime UpdatedAt

    );

    /// <summary>
    /// Represents a request to update a favorite.
    /// </summary>
    public record UpdateFavouriteRequest(
        /// <summary>
        /// Gets the id of the favorite to update.
        /// </summary>
        long Id,

        /// <summary>
        /// Gets or sets the new favorite label.
        /// </summary>
        string Label,

        /// <summary>
        /// Gets or sets the new favorite link.
        /// </summary>
        string Link,

        /// <summary>
        /// Gets or sets the new id of the category associated with the favorite.
        /// </summary>
        long CategoryId
    );

    /// <summary>
    /// Represents the response of a category.
    /// </summary>
    public record CategoryResponse(
        /// <summary>
        /// Gets the category id.
        /// </summary>
        long Id,

        /// <summary>
        /// Gets the category label.
        /// </summary>
        string Label
    );

    /// <summary>
    /// Represents a category creation request.
    /// </summary>
    public record CreateCategoryRequest(
        /// <summary>
        /// Gets or sets the category label.
        /// </summary>
        string Label
    );

    /// <summary>
    /// Represents a request to update a category.
    /// </summary>
    public record UpdateCategoryRequest(
        /// <summary>
        /// Gets the category id to update.
        /// </summary>
        long Id,

        /// <summary>
        /// Gets or sets the new category label.
        /// </summary>
        string Label
    );
}
