using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Playback;
using MediaManager.Player;
using Radeoh.Models;

namespace Radeoh.ViewModels
{
    public class PlayerViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IUserDialogs _dialogs = UserDialogs.Instance;
        private IMediaManager _mediaManager = CrossMediaManager.Current;
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
            _mediaManager.StateChanged += CurrentOnStateChanged;
            _mediaManager.MediaItemChanged += CurrentOnMediaItemChanged;
            _mediaManager.MediaItemFailed += CurrentOnMediaItemFailed;
        }

        public void AttachStateChangedHandler(StateChangedEventHandler eventHandler)
        {
            _mediaManager.StateChanged += eventHandler;
        }

        public async Task InitPlay()
        {
            Debug.Print("PlayerViewModel::InitPlay");
            var uri = Station.StreamUrl;

            if (_mediaManager.IsPlaying())
            {
                if (_currentItem.MediaUri == uri)
                {
                    Debug.Print($"Already playing {uri} skipping.");
                    return;
                }
            }
            
            _currentItem = await _mediaManager.Play(uri);
            _mediaManager.AutoPlay = true;

            _currentItem.Title = Station.Title;
            _currentItem.Image = Station.CachedImageSource;
        }

        private void CurrentOnStateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case MediaPlayerState.Buffering:
                    Debug.Print("Player: MediaPlayerState.Buffering");
                    break;
                case MediaPlayerState.Loading:
                    Debug.Print("Player: MediaPlayerState.Loading");
                    break;
                case MediaPlayerState.Paused:
                    Debug.Print("Player: MediaPlayerState.Paused");
                    break;
                case MediaPlayerState.Stopped:
                    Debug.Print("Player: MediaPlayerState.Stopped");
                    break;
                case MediaPlayerState.Failed:
                    Debug.Print("Player: MediaPlayerState.Failed", this._currentItem);
                    break;
            }
        }

        private void CurrentOnMediaItemChanged(object sender, MediaItemEventArgs e)
        {
            CrossMediaManager.Current.Play(e.MediaItem.MediaUri);
        }

        private void CurrentOnMediaItemFailed(object sender, MediaItemFailedEventArgs e)
        {
            _dialogs.AlertAsync($"Stream failed due to\n{e.Message}", "Ok");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.Print($"OnPropertyChanged({propertyName})");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }
    }
}