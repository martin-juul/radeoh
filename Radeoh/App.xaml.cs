using MediaManager;
using Radeoh.DAL;
using Radeoh.Views;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Radeoh
{
    public partial class App : Application
    {
        private static RadeohDatabase _database;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
        public App()
        {
            InitializeComponent();
            _logger.Debug("Initialized component");
            Current.UserAppTheme = OSAppTheme.Dark;

            CrossMediaManager.Current.Init();
            
            MainPage = new NavigationPage(new StationListView());
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
            VersionTracking.Track();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}