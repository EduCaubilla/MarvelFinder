using System.Threading.Tasks;
using System.Windows.Input;
using MarvelFinder.Base;
using MarvelFinder.Models;
using Xamarin.Forms;

namespace MarvelFinder.Features.ComicDetails
{
	public class ComicDetailsViewModel : BaseViewModel
	{

        public ICommand AddFavoriteCommand => new Command<MarvelComicItem>(async (item) => await AddFavoriteToList(item));

        public ComicDetailsViewModel(INavigation navigation) : base(navigation)
		{
           CheckComicData();
        }

        private void CheckComicData()
        {
            if (SelectedItem == null) return;
        }

        /// <summary>
        /// Calls to the BaseViewModel to add the item to the main favorites list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task AddFavoriteToList(MarvelComicItem item)
        {
            await AddFavorite(item);
        }
    }
}

