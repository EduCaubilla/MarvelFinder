using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MarvelFinder.Features.ComicDetails;
using MarvelFinder.Models;
using Xamarin.Forms; 

namespace MarvelFinder.Base
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public INavigation Navigation { get; set; }

		public static MarvelComicItem _selectedItem;
		public MarvelComicItem SelectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;
				RaiseOnPropertyChanged();
			}
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get => _isBusy;
			set
			{
                _isBusy = value;
				RaiseOnPropertyChanged();
			}
		}

        public static ObservableCollection<MarvelComicItem> _favoritesList;
        public ObservableCollection<MarvelComicItem> FavoritesList
        {
            get => _favoritesList;
            set
            {
                _favoritesList = value;
                RaiseOnPropertyChanged();
            }
        }

        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        /// <summary>
        /// Saves the favorite item in the local database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddFavorite(MarvelComicItem item)
        {
            item.IsFavorite = true;

            if(FavoritesList.Count > 0)
            {
                var favItemIndex = FavoritesList.ToList().Select(fav => fav.Title == item.Title).First();
                if (favItemIndex)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Item already saved",
                        $"The comic with title \"{item.Title}\" is already on your favorites list.",
                        "Accept");
                    return;
                }
            }

            var itemSavedId = await App.Database.SaveFavorite(item);

            if (itemSavedId > 0)
            {
                var FavoritesAux = await App.Database.GetFavoritesListAsync();
                FavoritesList = new ObservableCollection<MarvelComicItem>(FavoritesAux);

                await Application.Current.MainPage.DisplayAlert(
                    "Item saved successfully",
                    $"The comic with title \"{item.Title}\" was added to the favorites list.",
                    "Accept");
            }
            else
            {
                Debug.WriteLine($"Error on saving item to database");

                await Application.Current.MainPage.DisplayAlert(
                            "Database Error Saving",
                            $"The comic with title \"{item.Title}\" couldn't be saved." +
                            "\nPlease try again later or restart the app.",
                            "Accept");
            }

        }

        /// <summary>
        /// Deletes the given item from the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RemoveFavorite(MarvelComicItem item)
        {
            MarvelComicItem itemInFavList = null;

            if (FavoritesList.Count > 0)
            {
                itemInFavList = FavoritesList.ToList().First(fav => fav.Title == item.Title);
            }

            if (itemInFavList != null)
            {
                item.IsFavorite = false;

                var result = await App.Database.DeleteFavorite(itemInFavList.Id);

                if (result > 0)
                {
                    var FavoritesAux = FavoritesList;
                    FavoritesAux.ToList().Remove(item);
                    FavoritesList = new ObservableCollection<MarvelComicItem>(FavoritesAux);

                    await Application.Current.MainPage.DisplayAlert(
                    "Item removed",
                    $"The comic with title \"{item.Title}\" was deleted to the favorites list.",
                    "Accept");
                }
                else
                {
                    Debug.WriteLine($"Error on deleting item from database");

                    await Application.Current.MainPage.DisplayAlert(
                            "Database Error Deleting",
                            $"The comic with title \"{item.Title}\" couldn't be deleted." +
                            "\nPlease try again later or restart the app.",
                            "Accept");
                }
            }
        }

        /// <summary>
        /// Navigation to the comic detail view
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task NavigateToComicDetail(MarvelComicItem item)
        {
            if(SelectedItem == null || SelectedItem != item)
            {
                SelectedItem = item;
            }

            try
            {
                await Navigation.PushAsync(new ComicDetailsPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error on navigation -> {ex.Message}");
            }
        }

        #region INofityPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaiseOnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

        #endregion
    }
}

