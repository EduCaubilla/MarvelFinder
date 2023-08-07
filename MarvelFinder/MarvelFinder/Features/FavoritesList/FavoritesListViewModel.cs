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
        public ObservableCollection<MarvelComicItem> _favoritesListView = new ObservableCollection<MarvelComicItem>();
        public ObservableCollection<MarvelComicItem> FavoritesListView
        {
            get => _favoritesListView;
            set
            {
                _favoritesListView = value;
                RaiseOnPropertyChanged();

                IsListEmpty = _favoritesListView.Count == 0;
            }
        }

        private bool _isListEmpty;
        public bool IsListEmpty
        {
            get => _isListEmpty;
            set
            {
                _isListEmpty = value;
                RaiseOnPropertyChanged();
            }
        }

        public ICommand RemoveFavoriteCommand => new Command<MarvelComicItem>(async (item) => await RemoveFavoriteItem(item));

        public ICommand NavigateToComicDetailCommand => new Command<MarvelComicItem>(async (item) => await NavigateToComicDetail(item));


        public FavoritesListViewModel(INavigation navigation) : base(navigation)
		{
		}

        public async Task OnAppearing()
        {
            IsBusy = true;
            await LoadFavoriteslistForView();
            IsBusy = false;
        }

        private async Task RemoveFavoriteItem(MarvelComicItem item)
        {
            await RemoveFavorite(item);
            await LoadFavoriteslistForView();
        }

        private async Task LoadFavoriteslistForView()
        {
            FavoritesListView = new ObservableCollection<MarvelComicItem>();

            var favoritesListDB = await App.Database.GetFavoritesListAsync();

            if (favoritesListDB.Count == 0)
            {
                FavoritesListView = new ObservableCollection<MarvelComicItem>();
                return;
            }

            favoritesListDB.ForEach(fav => fav.IsFavorite = true);

            FavoritesListView = new ObservableCollection<MarvelComicItem>(favoritesListDB);
            FavoritesList = FavoritesListView;
        }
    }
}
