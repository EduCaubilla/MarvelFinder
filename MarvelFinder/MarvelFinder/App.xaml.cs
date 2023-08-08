using MarvelFinder.Data;
using MarvelFinder.Features.ComicList;
using MarvelFinder.Utils;
using Xamarin.Forms;

namespace MarvelFinder
{
    public partial class App : Application
    {
        /// <summary>
        /// Local database connection - singleton
        /// </summary>
        private static Database database;
        public static Database Database
        {
            get
            {
                if (database == null) database = new Database(Constants.DatabasePath);
                return database;
            }
        }

        public App ()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ComicListPage());
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

