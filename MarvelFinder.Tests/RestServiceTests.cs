using MarvelFinder.Services;
using MarvelFinder.Utils;

namespace MarvelFinder.Tests
{
    [TestFixture]
    public class RestServiceTests
	{

        [Test]
        public async Task RestService_GetComicList_Returns_Correct_List()
        {
            //Arrange
            var service = new RestService();

            //Act
            var response = await service.GetComicList();

            //Assert
            Assert.NotNull(response, "The API response for basic request is null.");
            Assert.NotZero(response.Count, "The API response for basic request returns 0 items.");
            Assert.That(response.Count, Is.EqualTo(Constants.API_REQUEST_LIMIT),
                $"The API response for basic request is less than the request limit -> {Constants.API_REQUEST_LIMIT}");
        }

        [Test]
        public async Task RestService_GetComicList_Returns_Correct_SearchList()
        {
            //Arrange
            var service = new RestService();

            //Act
            var response = await service.GetComicList("Doctor Strange");

            //Assert
            Assert.IsNotNull(response, "The API response for search request \"Doctor Strange\" is null.");
            Assert.NotZero(response.Count, "The API response for search request \"Doctor Strange\" returns 0 items.");
            Assert.That(response.Count, Is.EqualTo(Constants.API_REQUEST_LIMIT),
                $"The API response for search request \"Doctor Strange\" is less than the request limit -> {Constants.API_REQUEST_LIMIT}");
        }
    }
}
