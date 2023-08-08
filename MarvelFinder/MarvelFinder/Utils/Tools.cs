using System.Security.Cryptography;
using System.Text;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace MarvelFinder.Utils
{
	public class Tools
	{
        public bool IsConnected;

        public Tools()
		{
            IsConnected = CrossConnectivity.Current.IsConnected;
        }

        public static string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public async void DisplayMessage(string title, string content, string buttonAccept)
        {
            if (Application.Current == null) return;
            await Application.Current.MainPage.DisplayAlert(title, content, buttonAccept);
        }
    }
}

