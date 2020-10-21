using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Radeoh.Models;
using Radeoh.Support;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationListView : ContentPage
    {
        ViewModels.StationListViewModel _viewModel = new ViewModels.StationListViewModel();
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public StationListView()
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
            var station = _viewModel.Stations[e.ItemIndex];
            await Navigation.PushAsync(new Player(station));
        }
    }
}