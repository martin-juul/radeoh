using System;
using System.Diagnostics;
using System.Threading;
using MediaManager;
using MediaManager.Player;
using Radeoh.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        private string _stateStr = "";
        private PlayerViewModel _playerViewModel;
        private CancellationToken _cancellationToken;

        public Player(ref PlayerViewModel playerViewModel)
        {
            InitializeComponent();
            _playerViewModel = playerViewModel;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            IsBusy = true;
            base.OnAppearing();

            StationImage.Source = _playerViewModel.Station.CachedImageSource;
            LabelMediaDetails.Text = _playerViewModel.Station.Name;
            LabelMediaGenre.Text = _playerViewModel.Station.Genre;

            await _playerViewModel.InitPlay();

            _cancellationToken = new CancellationToken();

            CrossMediaManager.Current.Reactive().State
                .Subscribe(ParseState, _cancellationToken);

            IsBusy = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var token = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken);
            token.Cancel();
        }

        public async void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            await _playerViewModel.InitPlay();
        }

        private void ParseState(MediaPlayerState state)
        {
            var isPlaying = false;

            switch (state)
            {
                case MediaPlayerState.Buffering:
                    _stateStr = "Buffering";
                    isPlaying = true;
                    break;
                case MediaPlayerState.Failed:
                    _stateStr = "Failed";
                    break;
                case MediaPlayerState.Loading:
                    _stateStr = "Loading";
                    isPlaying = true;
                    break;
                case MediaPlayerState.Paused:
                    _stateStr = "Paused";
                    break;
                case MediaPlayerState.Playing:
                    _stateStr = "Playing";
                    isPlaying = true;
                    break;
                case MediaPlayerState.Stopped:
                    _stateStr = "Stopped";
                    break;
            }
            
            Debug.WriteLine($"MediaPlayerState: {_stateStr}");

            LabelState.Text = _stateStr;
            BtnPlayPause.Text = isPlaying ? "Pause" : "Play";
        }
    }
}