using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Playback;
using MediaManager.Player;
using NLog;
using Radeoh.Models;
using Radeoh.Support;

namespace Radeoh.ViewModels
{
    public sealed class PlayerViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserDialogs _dialogs = UserDialogs.Instance;
        private readonly IMediaManager _mediaManager = CrossMediaManager.Current;
        private IMediaItem _currentItem;

        /// <summary>
        /// Apparently C# is retarded, so you need a backing field,
        /// if you want to customize your getter/setter. Otherwise
        /// you'll run into "accessor is recursive on all execution paths".
        /// </summary>
        private Station _station;

        public Station Station
        {
            get => _station;
            set
            {
                if (value == _station) return;
                _station = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PlayerViewModel()
        {
            Debug.Print("PlayerViewModel: constructed");
            ConfigureMediaManager();
        }

        public async Task InitPlay()
        {
            Debug.Print("PlayerViewModel::InitPlay");
            var uri = Station.StreamUrl;

            if (await DidToggleOnSameUri(uri)) return;

            if (_mediaManager.IsPlaying())
            {
                await _mediaManager.Stop();
            }
            
            // We got a new uri, so we're changing the stream
            _currentItem = await _mediaManager.Play(uri);

            _currentItem.Title = Station.Title;
            _currentItem.Image = Station.CachedImageSource;
            _mediaManager.Notification.UpdateNotification();
            ConfigureNotification();
        }

        private async Task<bool> DidToggleOnSameUri(string uri)
        {
            if (_mediaManager.IsPlaying())
            {
                // If MediaManager is playing, we know _currentItem exist.
                if (_currentItem.MediaUri == uri)
                {
                    Debug.Print($"Already playing {uri} - assuming toggle stop.");
                    await _mediaManager.Pause();
                    return true;
                }
            }

            if (_mediaManager.Queue.HasCurrent)
            {
                if (_currentItem.MediaUri == uri)
                {
                    Debug.Print("Unpausing");
                    await _mediaManager.Play();
                    return true;
                }
            }

            return false;
        }

        private void ConfigureMediaManager()
        {
            _mediaManager.StateChanged += CurrentOnStateChanged;
            _mediaManager.MediaItemChanged += CurrentOnMediaItemChanged;
            _mediaManager.MediaItemFailed += CurrentOnMediaItemFailed;
            
            _mediaManager.AutoPlay = true;
            _mediaManager.ClearQueueOnPlay = true;
            _mediaManager.RetryPlayOnFailed = true;
            _mediaManager.MaxRetryCount = 5; 
        }

        private void ConfigureNotification()
        {
            _mediaManager.Notification.ShowNavigationControls = false;
            _mediaManager.Notification.ShowPlayPauseControls = true;
            _mediaManager.Notification.Enabled = true;
        }

        private void CurrentOnStateChanged(object sender, StateChangedEventArgs e)
        {
            var state = "Unknown";
            
            switch (e.State)
            {
                case MediaPlayerState.Buffering:
                    Debug.Print("Player: MediaPlayerState.Buffering");
                    state = "Buffering";
                    break;
                case MediaPlayerState.Loading:
                    Debug.Print("Player: MediaPlayerState.Loading");
                    state = "Loading";
                    break;
                case MediaPlayerState.Paused:
                    Debug.Print("Player: MediaPlayerState.Paused");
                    state = "Paused";
                    break;
                case MediaPlayerState.Stopped:
                    Debug.Print("Player: MediaPlayerState.Stopped");
                    state = "Stopped";
                    break;
                case MediaPlayerState.Failed:
                    Debug.Print("Player: MediaPlayerState.Failed", _currentItem);
                    state = "Failed";
                    break;
            }
            
            _logger.Info($"CurrentOnStateChanged: {state}");
        }

        private void CurrentOnMediaItemChanged(object sender, MediaItemEventArgs e)
        {
            _logger.Info($"CurrentOnMediaItemChanged: {e.MediaItem}");
            CrossMediaManager.Current.Play(e.MediaItem.MediaUri);
        }

        private void CurrentOnMediaItemFailed(object sender, MediaItemFailedEventArgs e)
        {
            _logger.Error($"CurrentOnMediaItemFailed: {e}");
            _dialogs.AlertAsync($"Stream failed due to\n{e.Message}", "Ok");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.Print($"OnPropertyChanged: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }
    }
}