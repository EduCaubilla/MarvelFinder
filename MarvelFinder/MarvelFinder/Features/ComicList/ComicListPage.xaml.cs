using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelFinder.Features.ComicList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComicListPage : ContentPage
	{	
		public ComicListPage ()
		{
			InitializeComponent();
			BindingContext = new ComicListViewModel(Navigation);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as ComicListViewModel;

			await vm.OnAppearing();
		}

        void ComicList_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
			var vm = BindingContext as ComicListViewModel;
			vm.NavigateToComicDetailCommand.Execute(null);
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
			var vm = BindingContext as ComicListViewModel;
			vm.LoadFavoriteslist();
        }
    }
}

