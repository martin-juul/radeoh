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
            // Forces dark mode
            Current.UserAppTheme = OSAppTheme.Dark;
            _logger.Debug("Set Current.UserAppTheme to OSAppTheme.Dark");
            
            InitializeComponent();
            _logger.Debug("Initialized component");

            Device.SetFlags(new string[] { "MediaElement_Experimental" });
            
            MainPage = new MainPage();
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
            _logger.Debug("App::OnStart");
            VersionTracking.Track();
        }

        protected override void OnSleep()
        {
            _logger.Debug("App::OnSleep");
        }

        protected override void OnResume()
        {
            _logger.Debug("App::OnResume");
        }
    }
}