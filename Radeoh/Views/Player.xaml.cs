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
using PositionChangedEventArgs = MediaManager.Playback.PositionChangedEventArgs;

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
            this.Station = station;
            this.BindingContext = this;
            
            CrossMediaManager.Current.StateChanged += Current_OnStateChanged;
            CrossMediaManager.Current.MediaItemChanged += Current_MediaItemChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!CrossMediaManager.Current.IsPrepared())
            {
                await InitPlay();

                // Set up Player Preferences
                CrossMediaManager.Current.AutoPlay = true;
            }
            else
            {
                SetupCurrentMediaDetails(CrossMediaManager.Current.Queue.Current);
                SetupCurrentMediaPlayerState(CrossMediaManager.Current.State);
            }
        }

        private async Task InitPlay()
        {
            this._currentItem = await CrossMediaManager.Current.Play(this.Station.StreamUrl);
        }

        private void SetupCurrentMediaDetails(IMediaItem currentMediaItem)
        {
            LabelMediaDetails.Text = this.Station.Title;
        }

        private void SetupCurrentMediaPlayerState(MediaPlayerState currentPlayerState)
        {
            // LabelPlayerStatus.Text = $"{currentPlayerState.ToString().ToUpper()}";

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

        private async void PlayPauseButton_Clicked(object sender, EventArgs e)
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
            Device.BeginInvokeOnMainThread(() => 
            {
                SetupCurrentMediaPlayerState(e.State); 
            });
        }

    }
}