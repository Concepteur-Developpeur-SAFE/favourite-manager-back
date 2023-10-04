namespace FavouriteManager.Exception
{
    /// <summary>
    /// Exception thrown when a validation fails.
    /// This exception is typically used to indicate that a data validation has failed, for example, when creating an entity.
    /// </summary>
    public class ValidationException : IOException
    {
        /// <summary>
        /// Initializes a new instance of the ValidationException.
        /// </summary>
        public ValidationException() : base() { }
    }
}
