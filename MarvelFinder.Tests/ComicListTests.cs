using MarvelFinder.Features.ComicList;
using MarvelFinder.Services;
using MarvelFinder.Utils;

namespace MarvelFinder.Tests
{
	[TestFixture]
	public class ComicListTests
	{
        private static RestService _restService = new RestService();

		[Test]
		public async Task ComicList_SearchComicList_Returns_Correct_List()
		{
			// Arrange
            var vm = new ComicListViewModel(null, _restService);

			// Act
			await vm.SearchComics().ConfigureAwait(true);

            // Assert
            Assert.NotNull(vm.ComicList, "The ViewModel's function SearchComics returns null.");
            Assert.NotZero(vm.ComicList.Count, "The ViewModel's function SearchComics returns 0 items.");
            Assert.That(vm.ComicList.Count, Is.EqualTo(Constants.API_REQUEST_LIMIT),
                $"The ViewModel's function SearchComics returns less than the limit of {Constants.API_REQUEST_LIMIT} items.");
		}

        [Test]
        public async Task ComicList_SearchComicList_Returns_Correct_SearchList()
        {
            // Arrange
            var vm = new ComicListViewModel(null, _restService);

            // Act
            await vm.SearchComics("Hulk").ConfigureAwait(true);

            // Assert
            Assert.NotNull(vm.ComicList, "The ViewModel's function SearchComics with query \"Hulk\" returns null.");
            Assert.NotZero(vm.ComicList.Count, "The ViewModel's function SearchComics with query \"Hulk\" returns 0 items.");
            Assert.That(vm.ComicList.Count, Is.EqualTo(Constants.API_REQUEST_LIMIT),
                $"The ViewModel's function SearchComics with query \"Hulk\" returns less than {Constants.API_REQUEST_LIMIT} items.");
        }
    }
}

