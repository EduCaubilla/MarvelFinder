using System;
using System.Collections.Generic;
using MarvelFinder.Features.ComicList;
using Xamarin.Forms;

namespace MarvelFinder.Features.ComicDetails
{	
	public partial class ComicDetailsPage : ContentPage
	{	
		public ComicDetailsPage()
		{
			InitializeComponent();
            BindingContext = new ComicDetailsViewModel(Navigation);
        }
    }
}

