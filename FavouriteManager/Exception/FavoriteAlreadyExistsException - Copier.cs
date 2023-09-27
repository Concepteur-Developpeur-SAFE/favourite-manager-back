namespace FavouriteManager.Exception
{
    public class FavoriteAlreadyExistsException : IOException
    {
        public FavoriteAlreadyExistsException(string message) : base(message) { }
    }
}
