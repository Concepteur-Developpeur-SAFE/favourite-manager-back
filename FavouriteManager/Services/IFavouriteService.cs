﻿using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services
{
    public interface IFavouriteService
    {
        long Test(int id);
        List<Favourite> FilterByCategory(string category);
    }

}
