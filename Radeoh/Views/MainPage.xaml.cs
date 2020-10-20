using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Radeoh.Models;
using Radeoh.Support;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        ViewModels.MainPageViewModel _viewModel = new ViewModels.MainPageViewModel();
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.FetchStationsCommand.Execute(null);
        }

        async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            if (e.GetType() == typeof(Station))
            {
                Station station = e.Item as Station;
                await Navigation.PushAsync(new Player(station));
            }
            else
            {
                e.Json();
                _logger.Error("Wrong item", e.Item);
            }
        }
    }
}