namespace FavouriteManager.Exception
{
    public class ValidationException : IOException
    {
        public ValidationException(string message) : base(message) { }
    }
}
