using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MarvelFinder.Base;
using MarvelFinder.Features.ComicDetails;
using MarvelFinder.Features.ComicList;
using MarvelFinder.Models;
using Xamarin.Forms;

namespace MarvelFinder.Features.FavoritesList
{
	public class FavoritesListViewModel : BaseViewModel
	{
        public ObservableCollection<MarvelComicItem> _favoritesListView;
        public ObservableCollection<MarvelComicItem> FavoritesListView
        {
            get => _favoritesListView;
            set
            {
                _favoritesListView = value;
                RaiseOnPropertyChanged();
            }
        }

        public ICommand RemoveFavoriteCommand => new Command<MarvelComicItem>(async (item) => await RemoveFavoriteItem(item));

        public ICommand NavigateToComicDetailCommand => new Command(async () => await NavigateToComicDetail());


        public FavoritesListViewModel(INavigation navigation) : base(navigation)
		{
		}

        public async Task OnAppearing()
        {
            IsBusy = true;
            LoadFavoriteslistForView();
            IsBusy = false;
        }

        private async Task RemoveFavoriteItem(MarvelComicItem item)
        {
            await RemoveFavorite(item);
        }

        private async void LoadFavoriteslistForView()
        {
            var favoritesListDB = await App.Database.GetFavoritesListAsync();

            favoritesListDB.ForEach(fav => fav.IsFavorite = true);

            FavoritesListView = new ObservableCollection<MarvelComicItem>(favoritesListDB);
            FavoritesList = FavoritesListView;
        }
    }
}
