using System;
using System.IO;
using Foundation;
using Xamarin.Forms;

namespace MarvelFinder.Utils
{
	public class Constants
	{
        public static string API_TIMESTAMP = DateTime.Now.TimeOfDay.Ticks.ToString();

        public const string BASE_URI = "https://gateway.marvel.com/v1/public/comics?";

        public const string API_KEY = "86340172b5b3d3661d131ae626154fa3";

        public const string API_PRIVATE_KEY = "754c6e1a370e72abb28f6a99fdfb586608e01940";

		public const int API_REQUEST_LIMIT = 50;

        public Constants()
		{

		}

        public const string DatabaseFilename = "MarvelFinderAppData.db3";

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        //public static void InitBasePath()
        //{
        //    if (Device.RuntimePlatform == Device.iOS)
        //    {
        //        BASE_PATH = Path.Combine(NSBundle.MainBundle.PathForResource("MarvelFinderAppData", "db3"));
        //    }
        //    else if (Device.RuntimePlatform == Device.Android)
        //    {
        //        BASE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MarvelFinderAppData.db3");
        //    }
        //}

    }
}

