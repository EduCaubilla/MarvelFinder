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
    }
}

