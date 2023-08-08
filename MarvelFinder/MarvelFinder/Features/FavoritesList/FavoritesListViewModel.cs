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
            await LoadFavoriteslistForView();
        }

        /// <summary>
        /// Calls to base to remove an item from the favorites main list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task RemoveFavoriteItem(MarvelComicItem item)
        {
            await RemoveFavorite(item);
            await LoadFavoriteslistForView();
        }

        /// <summary>
        /// Gets the list of favorites from the local database to show in the view
        /// </summary>
        /// <returns></returns>
        private async Task LoadFavoriteslistForView()
        {
            IsBusy = true;

            FavoritesListView = new ObservableCollection<MarvelComicItem>();

            var favoritesListDB = await App.Database.GetFavoritesListAsync();

            if (favoritesListDB.Count == 0)
            {
                FavoritesListView = new ObservableCollection<MarvelComicItem>();
                IsBusy = false;
                return;
            }

            favoritesListDB.ForEach(fav => fav.IsFavorite = true);

            FavoritesListView = new ObservableCollection<MarvelComicItem>(favoritesListDB);
            FavoritesList = FavoritesListView;

            IsBusy = false;
        }
    }
}
