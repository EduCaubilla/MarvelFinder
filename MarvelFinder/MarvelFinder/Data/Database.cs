using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MarvelFinder.Models;
using SQLite;

namespace MarvelFinder.Data
{
	public class Database
	{
		private readonly SQLiteAsyncConnection _database;

		public Database(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<MarvelComicItem>().Wait();
		}

		public Task<List<MarvelComicItem>> GetFavoritesListAsync()
		{
			return _database.Table<MarvelComicItem>().ToListAsync();
        }

		public Task<int> SaveFavorite(MarvelComicItem favToSave)
		{
			return _database.InsertAsync(favToSave);
		}

		public Task<int> DeleteFavorite(int favId)
		{
			return _database.DeleteAsync<MarvelComicItem>(favId);
		}
	}
}
