using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Playback;
using MediaManager.Player;
using Radeoh.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        public Station Station;
        private IMediaItem _currentItem;

        public Player(Station station)
        {
            InitializeComponent();
            Station = station;
            BindingContext = this;

            CrossMediaManager.Current.StateChanged += Current_OnStateChanged;
            CrossMediaManager.Current.MediaItemChanged += Current_MediaItemChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            StationImage.Source = Station.SecureImageUrl;
            LabelMediaDetails.Text = Station.Title;

            await InitPlay();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async Task InitPlay()
        {
            var uri = Station.StreamUrl;
            _currentItem = await CrossMediaManager.Current.Play(uri);
            CrossMediaManager.Current.AutoPlay = true;

            _currentItem.Title = Station.Title;
            _currentItem.ImageUri = Station.SecureImageUrl;
        }

        private void SetupCurrentMediaDetails(IMediaItem currentMediaItem)
        {
            CrossMediaManager.Current.Play(currentMediaItem.MediaUri);
        }

        private void SetupCurrentMediaPlayerState(MediaPlayerState currentPlayerState)
        {
            if (currentPlayerState == MediaManager.Player.MediaPlayerState.Loading)
            {
                UserDialogs.Instance.Loading("Loading..");
            }
            else if (currentPlayerState == MediaManager.Player.MediaPlayerState.Playing
                     && CrossMediaManager.Current.Duration.Ticks > 0)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void Current_MediaItemChanged(object sender, MediaItemEventArgs e)
        {
            SetupCurrentMediaDetails(e.MediaItem);
        }

        public async void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMediaManager.Current.IsPrepared())
            {
                await InitPlay();
            }
            else
            {
                await CrossMediaManager.Current.PlayPause();
            }
        }

        private void Current_OnStateChanged(object sender, StateChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => { SetupCurrentMediaPlayerState(e.State); });
        }
    }
}