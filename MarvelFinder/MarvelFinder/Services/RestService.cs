using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MarvelFinder.Models;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using MarvelFinder.Mapper;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Diagnostics;
using MarvelFinder.Utils;

namespace MarvelFinder.Services
{
	public class RestService : IRestService
	{
        private Constants _constants;
        public Constants Constants
        {
            get => _constants;
            set
            {
                _constants = value;
            }
        }

        public RestService()
		{
            _constants = new Constants();
		}

        public async Task<List<MarvelComicItem>> GetComicList(string searchValue = "")
        {
            var stringToHash = Constants.API_TIMESTAMP + Constants.API_PRIVATE_KEY + Constants.API_KEY;

            var baseUri = Constants.BASE_URI;
            var paramTs = $"ts={Constants.API_TIMESTAMP}";
            var apiKey = $"&apikey={Constants.API_KEY}";

            var hash = CreateMD5Hash(stringToHash);
            var paramHash = $"&hash={hash}";

            var orderBy = "&orderBy=-focDate";
            var noVariants = "&noVariants=true";
            var limit = "&limit=100";

            var reqUri = $"{baseUri}{paramTs}{apiKey}{paramHash}{orderBy}{noVariants}{limit}";

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTitle = $"&title={searchValue}";
                var titleStartsWith = $"&titleStartsWith={searchValue}";
                reqUri = $"{baseUri}{paramTs}{apiKey}{paramHash}{orderBy}{noVariants}{limit}{searchTitle}{titleStartsWith}";
            }

            Debug.WriteLine($"URL -> {reqUri}");

            using (HttpClient client = new HttpClient())
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    Uri requestUri = new Uri(reqUri);
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(requestUri);

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            JObject parsedContent = JObject.Parse(content);

                            var restList = parsedContent["data"]["results"].ToString();

                            var result = JsonConvert.DeserializeObject<List<MarvelComicItemDTO>>(restList);

                            if (result != null && result.Count > 0)
                            {
                                var comicList = new List<MarvelComicItem>();
                                foreach (var comic in result)
                                {
                                    var comicMapped = comic.MapToModel();
                                    comicList.Add(comicMapped);
                                }
                                return comicList;
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(
                            "Request error",
                            "There was an error calling the API. " +
                            "\nPlease try again later.",
                            "Accept");
                            Debug.WriteLine("API call failed: " + response);
                        }
                    }
                    catch(Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Server error",
                            "No response received from the server. " +
                            "\nPlease try again later.",
                            "Accept");
                        Debug.WriteLine("API call failed: " + ex);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                            "Network error",
                            "Network service was not detected. " +
                            "\nPlease check you connection.",
                            "Accept");
                }
            }

            return null;
        }

        public string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

