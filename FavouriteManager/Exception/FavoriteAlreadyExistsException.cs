namespace FavouriteManager.Exception
{
    /// <summary>
    /// Exception thrown when an attempt is made to create a favorite that already exists.
    /// This exception is typically used to indicate that an attempt to create a favorite has failed due to the presence of a favorite with the same Link.
    /// </summary>
    public class FavoriteAlreadyExistsException : IOException
    {
        /// <summary>
        /// Initializes a new instance of the FavoriteAlreadyExistsException.
        /// </summary>
        public FavoriteAlreadyExistsException() : base() { }
    }
}
