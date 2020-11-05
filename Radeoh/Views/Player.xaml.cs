using System;
using System.Diagnostics;
using Radeoh.Models;
using Radeoh.Support;
using Radeoh.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        private PlayerViewModel _playerViewModel;

        public Player(ref PlayerViewModel playerViewModel)
        {
            InitializeComponent();
            _playerViewModel = playerViewModel;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            StationImage.Source = _playerViewModel.Station.CachedImageSource;
            LabelMediaDetails.Text = _playerViewModel.Station.Name;
            LabelMediaGenre.Text = _playerViewModel.Station.Genre;

            await _playerViewModel.InitPlay();
        }

        public async void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            await _playerViewModel.InitPlay();
        }
    }
}