using FavouriteManager.Data;
using System.Collections.Generic;
using System.Linq;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services.implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDBContext _appDbContext;
        public FavouriteService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public long Test(int id)
        {
            return _appDbContext.Test(id);
        }
    }
}
