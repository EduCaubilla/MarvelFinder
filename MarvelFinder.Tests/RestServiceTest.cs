using MarvelFinder.Services;

namespace MarvelFinder.Tests
{
    [TestFixture]
    public class RestServiceTest
	{

        [Test]
        public async Task RestService_GetComicList_Returns_Correct_List()
        {
            //Arrange
            var service = new RestService();

            //Act
            var response = await service.GetComicList();

            //Assert
            Assert.That(response.Count, Is.EqualTo(50), "The API response for basic request is null.");
        }

        [Test]
        public async Task RestService_GetComicList_Returns_Correct_SearchList()
        {
            //Arrange
            var service = new RestService();

            //Act
            var response = await service.GetComicList("Doctor Strange");

            //Assert
            Assert.IsNotNull(response, "The API response for search requeste is null.");
        }
    }
}
