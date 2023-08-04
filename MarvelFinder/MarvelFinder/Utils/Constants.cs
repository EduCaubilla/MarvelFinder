using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MarvelFinder.Utils
{
	public class Constants
	{
		public const string BASE_URI = "https://gateway.marvel.com/v1/public/comics?";

        public string API_TIMESTAMP = DateTime.Now.TimeOfDay.Ticks.ToString();

        public const string API_KEY = "86340172b5b3d3661d131ae626154fa3";

        public const string API_PRIVATE_KEY = "754c6e1a370e72abb28f6a99fdfb586608e01940";


        public Constants()
		{
		}

	}
}

