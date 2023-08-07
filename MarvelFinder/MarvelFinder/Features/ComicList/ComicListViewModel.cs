using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MarvelFinder.Base;
using MarvelFinder.Features.ComicDetails;
using MarvelFinder.Features.FavoritesList;
using MarvelFinder.Models;
using MarvelFinder.Services;
using Xamarin.Forms;

namespace MarvelFinder.Features.ComicList
{
	public class ComicListViewModel : BaseViewModel
	{

		private RestService _restService;

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


        public ComicListViewModel(INavigation navigation) : base(navigation)
		{
			_restService = new RestService();
		}

		public async Task OnAppearing()
		{
			if(ComicList == null || ComicList.Count == 0) await SearchComics();
			LoadFavoriteslist();
        }

		private async Task SearchComics(string searchValue = "")
		{
			if (ComicList != null && string.IsNullOrEmpty(searchValue)) return;

            ComicList = new ObservableCollection<MarvelComicItem>();

            IsBusy = true;

			var comicList = await _restService.GetComicList(searchValue);

			if(comicList == null || comicList.Count == 0)
			{
                await Application.Current.MainPage.DisplayAlert(
                            "Request empty",
                            $"Nothing found with the search \"{searchValue}\"." +
                            "\nPlease try again with a different text.",
                            "Accept");
            }
			else
			{
                ComicList = new ObservableCollection<MarvelComicItem>(comicList);
            }

			IsBusy = false;
		}

        public async void LoadFavoriteslist()
		{
            FavoritesList = new ObservableCollection<MarvelComicItem>();

            var favoritesListDB = await App.Database.GetFavoritesListAsync();

            favoritesListDB.ForEach(fav => fav.IsFavorite = true);

			if (favoritesListDB == null) return;

			FavoritesList = new ObservableCollection<MarvelComicItem>(favoritesListDB);

            CheckComicsListForFavs();
        }

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

        private async Task AddFavoriteToList(MarvelComicItem item)
        {
            await AddFavorite(item);
            CheckComicsListForFavs();
        }

        private async Task RemoveFavoriteFromList(MarvelComicItem item)
        {
            await RemoveFavorite(item);
            CheckComicsListForFavs();
        }
    }
}

