using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MarvelFinder.Base;
using MarvelFinder.Features.FavoritesList;
using MarvelFinder.Models;
using MarvelFinder.Services;
using Xamarin.Forms;

namespace MarvelFinder.Features.ComicList
{
	public class ComicListViewModel : BaseViewModel
	{

		private IRestService _restService;

        private ObservableCollection<MarvelComicItem> _comicList;
		public ObservableCollection<MarvelComicItem> ComicList
		{
			get => _comicList;
			set
			{
				_comicList = value;
				RaiseOnPropertyChanged();
			}
		}

        public ICommand NavigateToComicDetailCommand => new Command<MarvelComicItem>(async (item) => await NavigateToComicDetail(item));

		public ICommand SearchComicsCommand => new Command<string>(async (searchValue) => await SearchComics(searchValue));

		public ICommand ShowFavoritesListCommand => new Command(() => ShowFavorites());

        public ICommand AddFavoriteCommand => new Command<MarvelComicItem>(async (item) => await AddFavoriteToList(item));

        public ICommand RemoveFavoriteCommand => new Command<MarvelComicItem>(async (item) => await RemoveFavoriteFromList(item));


        public ComicListViewModel(INavigation navigation, IRestService restService) : base(navigation)
		{
			_restService = restService;
		}

		public async Task OnAppearing()
		{
			if(ComicList == null || ComicList.Count == 0) await SearchComics();
			LoadFavoriteslist();
        }

        /// <summary>
        /// Gets a list of marvel comics from the search and put it in the collection binded to the main view
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
		public async Task SearchComics(string searchValue = "")
		{
			if (ComicList != null && string.IsNullOrEmpty(searchValue)) return;

            ComicList = new ObservableCollection<MarvelComicItem>();

            IsBusy = true;

			var comicList = await _restService.GetComicList(searchValue);

			if(comicList == null || comicList.Count == 0)
			{
                //await Application.Current.MainPage.DisplayAlert(
                //            "Request empty",
                //            $"Nothing found with the search \"{searchValue}\"." +
                //            "\nPlease try again with a different text.",
                //            "Accept");
            }
			else
			{
                ComicList = new ObservableCollection<MarvelComicItem>(comicList);
            }

			IsBusy = false;
		}

        /// <summary>
        /// Gets the list of favorites form local db and put it in the collection binded to the view related
        /// </summary>
        public async void LoadFavoriteslist()
		{
            FavoritesList = new ObservableCollection<MarvelComicItem>();

            var favoritesListDB = await App.Database.GetFavoritesListAsync();

            favoritesListDB.ForEach(fav => fav.IsFavorite = true);

			if (favoritesListDB == null) return;

			FavoritesList = new ObservableCollection<MarvelComicItem>(favoritesListDB);

            CheckComicsListForFavs();
        }

        /// <summary>
        /// Cross check the favorites list with the search list to mark favs in the last one
        /// and disallow add only allowing remove from favorites list
        /// </summary>
        private void CheckComicsListForFavs()
        {
            var newComicList = ComicList.ToList();

            newComicList.ForEach(item => item.IsFavorite = false);

            if (FavoritesList.Count > 0)
            {
                FavoritesList.ToList().ForEach(fav =>
                {
                    newComicList.ForEach(comic =>
                    {
                        if (fav.Title == comic.Title)
                        {
                            comic.IsFavorite = true;
                        }
                    });
                });
            }

            ComicList = new ObservableCollection<MarvelComicItem>(newComicList);
        }

        /// <summary>
        /// Navigation to favorites list page
        /// </summary>
        private async void ShowFavorites()
		{
            try
            {
                await Navigation.PushAsync(new FavoritesListPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error on navigation -> {ex.Message}");
            }
        }

        /// <summary>
        /// Calls to the BaseViewModel to add the item to the main favorites list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task AddFavoriteToList(MarvelComicItem item)
        {
            await AddFavorite(item);
            CheckComicsListForFavs();
        }

        /// <summary>
        /// Calls to the BaseViewModel remove the item from the main favorites list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task RemoveFavoriteFromList(MarvelComicItem item)
        {
            await RemoveFavorite(item);
            CheckComicsListForFavs();
        }
    }
}

