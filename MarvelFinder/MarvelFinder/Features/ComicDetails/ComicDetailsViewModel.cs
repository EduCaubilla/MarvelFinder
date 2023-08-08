using MarvelFinder.Base;
using Xamarin.Forms;

namespace MarvelFinder.Features.ComicDetails
{
	public class ComicDetailsViewModel : BaseViewModel
	{

        public ComicDetailsViewModel(INavigation navigation) : base(navigation)
		{
           CheckComicData();
        }

        private void CheckComicData()
        {
            if (SelectedItem == null) return;
        }
    }
}

