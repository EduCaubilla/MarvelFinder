﻿using System.Diagnostics;
using MarvelFinder.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelFinder.Features.ComicList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComicListPage : ContentPage
	{
		private RestService restService = new RestService();

		public ComicListPage ()
		{
			InitializeComponent();
			BindingContext = new ComicListViewModel(Navigation, restService);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as ComicListViewModel;

			await vm.OnAppearing();
		}
    }
}

