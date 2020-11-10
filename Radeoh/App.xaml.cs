using Radeoh.DAL;
using Radeoh.Views;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Radeoh
{
    public partial class App : Application
    {
        private static DbContext _dbContext;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
        public App()
        {
            InitializeComponent();
            _logger.Debug("Initialized component");
            Current.UserAppTheme = OSAppTheme.Dark;

            MainPage = new NavigationPage(new StationListView());
        }
        
        public static DbContext DbContext => _dbContext ??= new DbContext();

        protected override void OnStart()
        {
            DbContext.Database.InitializeAsync().SafeFireAndForget(false);
            
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