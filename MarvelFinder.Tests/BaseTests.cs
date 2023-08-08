
using System.Collections.ObjectModel;
using MarvelFinder.Base;
using MarvelFinder.Models;

namespace MarvelFinder.Tests
{
	[TestFixture]
	public class BaseTests
	{
		[Test]
		public async Task Base_AddFavorite_Correct()
		{
			// Arrange
			var mockItem = new MarvelComicItem
			{
				Title = "Test Marvel Comic To Add",
				Id = 0,
				Description = "Test Marvel Comic To Add Description",
				PageCount = "10"
			};

			var vm = new BaseViewModel(null);
			vm.FavoritesList = new ObservableCollection<MarvelComicItem>();

			// Act
			await vm.AddFavorite(mockItem).ConfigureAwait(true);

			// Assert
			var savedItem = vm.FavoritesList.FirstOrDefault(item => item.Title == mockItem.Title);

			Assert.True(savedItem != null, "Item to save is not in the list of saved elements.");

			var itemSaved = vm.FavoritesList.First(item => item.Title == mockItem.Title) ?? mockItem;
			Assert.GreaterOrEqual(itemSaved.Id, 1, "Item to save has no id so it wasn't saved.");

			//Clean db
			foreach (var item in vm.FavoritesList)
			{
				if(item.Title == mockItem.Title)
				{
					await vm.RemoveFavorite(item);
				}
			}
		}

		[Test]
		public async Task Base_RemoveFavorite_Correct()
		{
			// Arrange
			var mockItem = new MarvelComicItem
			{
				Title = "Test Marvel Comic To Rmove",
				Id = 0,
				Description = "Test Marvel Comic To Rmove Description",
				PageCount = "11"
			};

			var vm = new BaseViewModel(null);
			vm.FavoritesList = new ObservableCollection<MarvelComicItem>();

			// Act
			await vm.AddFavorite(mockItem).ConfigureAwait(true);
			var savedCount = vm.FavoritesList.Count;

			await vm.RemoveFavorite(mockItem).ConfigureAwait(true);
			var removedCount = vm.FavoritesList.Count;

			// Assert
			var removedItem = vm.FavoritesList.FirstOrDefault(item => item.Title == mockItem.Title);
			Assert.True(removedItem == null,
				"Item to remove is still present in the database.");

			Assert.That(savedCount - removedCount, Is.EqualTo(1), "Item was not saved in the database.");
		}
	}
}

