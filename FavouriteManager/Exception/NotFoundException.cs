namespace FavouriteManager.Exception
{
    /// <summary>
    /// Exception thrown when a resource is not found.
    /// This exception is generally used to indicate that a specific entity was not found in the system.
    /// </summary>
    public class NotFoundException : IOException
    {
        /// <summary>
        /// Initializes a new instance of the NotFoundException.
        /// </summary>
        public NotFoundException() : base()
        {
        }
    }
}

