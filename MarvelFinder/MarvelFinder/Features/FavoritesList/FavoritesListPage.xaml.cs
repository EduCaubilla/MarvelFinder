using System;
using System.Collections.Generic;
using MarvelFinder.Features.ComicList;
using Xamarin.Forms;

namespace MarvelFinder.Features.FavoritesList
{	
	public partial class FavoritesListPage : ContentPage
	{	
		public FavoritesListPage ()
		{
			InitializeComponent ();
            BindingContext = new FavoritesListViewModel(Navigation);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as FavoritesListViewModel;

            await vm.OnAppearing();
        }

        void ComicList_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var vm = BindingContext as FavoritesListViewModel;
            vm.NavigateToComicDetailCommand.Execute(null);
        }
    }
}

