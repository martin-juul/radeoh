using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Radeoh.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        public Station Station;
        
        public Player(Station station)
        {
            InitializeComponent();
        }
        
        void OnPlayPauseButtonClicked(object sender, EventArgs args)
        {
            switch (MediaElement.CurrentState)
            {
                case MediaElementState.Closed:
                case MediaElementState.Stopped:
                case MediaElementState.Paused:
                    MediaElement.Play();
                    break;
                case MediaElementState.Playing:
                    MediaElement.Pause();
                    break;
                case MediaElementState.Opening:
                    UserDialogs.Instance.Loading("Loading");
                    break;
                case MediaElementState.Buffering:
                    UserDialogs.Instance.Loading("Buffering");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnStopButtonClicked(object sender, EventArgs args)
        {
            MediaElement.Stop();
        }
    }
}