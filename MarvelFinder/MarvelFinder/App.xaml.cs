using System;
using System.IO;
using MarvelFinder.Data;
using MarvelFinder.Features.ComicList;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MarvelFinder
{
    public partial class App : Application
    {

        //Inicio base de datos
        private static Database database;

        public static Database Database
        {
            get
            {
                var basePath = Path.Combine(FileSystem.AppDataDirectory, "FlightAppData.db3");
                if (database == null) database = new Database(basePath);

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

