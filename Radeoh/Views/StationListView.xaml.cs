using System;
using System.Diagnostics;
using NLog;
using Radeoh.DAL.Models;
using Radeoh.Models;
using Radeoh.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationListView : ContentPage
    {
        StationListViewModel _viewModel = new StationListViewModel();
        PlayerViewModel _playerViewModel = new PlayerViewModel();

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public StationListView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _logger.Debug("OnAppearing::FetchStationsCommand");
            _viewModel.FetchStationsCommand.Execute(null);
        }

        async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Debug.Print(e.Item.ToString());

            if (((ListView) sender).SelectedItem == null)
            {
                return;
            }

            var station = _viewModel.Stations[e.ItemIndex];
            _playerViewModel.Station = station;
            await Navigation.PushAsync(new Player(ref _playerViewModel));

            ((ListView) sender).SelectedItem = null;
        }

        private void SwipeItem_OnInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem) sender;
            var item = (Station) swipeItem.BindingContext;
            var fav = new Favorite {Slug = item.Slug};
            
            App.DbContext.favoriteRepository.SaveAsync(fav).SafeFireAndForget(false);

            if (swipeItem.BackgroundColor == Color.Gray)
            {
                swipeItem.BackgroundColor = Color.LightCoral;
                swipeItem.Text = "❤️";
                item.IsFavorite = false;
            }
            else
            {
                swipeItem.BackgroundColor = Color.Gray;
                swipeItem.Text = "💔";
                item.IsFavorite = true;
            }
        }
    }
}