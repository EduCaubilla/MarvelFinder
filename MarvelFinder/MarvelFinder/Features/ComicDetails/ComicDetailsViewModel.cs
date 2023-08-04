using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MarvelFinder.Base;
using MarvelFinder.Models;
using Xamarin.Essentials;
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

