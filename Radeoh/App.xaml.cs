using System;
using NLog;
using NLog.Fluent;
using Radeoh.DAL;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Radeoh.Services;
using Radeoh.Views;

namespace Radeoh
{
    public partial class App : Application
    {
        private static RadeohDatabase _database;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();

            _logger.Info("Application started");
        }

        public static RadeohDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new RadeohDatabase();
                }

                return _database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}