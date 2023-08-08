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

        private Tools _tools;

        public RestService()
		{
            _tools = new Tools();
		}

        /// <summary>
        /// Gets a list of comics from the marvel api
        /// </summary>
        /// <param name="searchValue">String with the searched word</param>
        /// <returns>List of comics</returns>
        public async Task<List<MarvelComicItem>> GetComicList(string searchValue = "")
        {
            var stringToHash = Constants.API_TIMESTAMP + Constants.API_PRIVATE_KEY + Constants.API_KEY;

            var baseUri = Constants.BASE_URI;
            var paramTs = $"ts={Constants.API_TIMESTAMP}";
            var apiKey = $"&apikey={Constants.API_KEY}"; 

            var hash = _tools.CreateMD5Hash(stringToHash);
            var paramHash = $"&hash={hash}"; 

            var orderBy = "&orderBy=-focDate";
            var noVariants = "&noVariants=true";
            var limit = "&limit=50";

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
    }
}

