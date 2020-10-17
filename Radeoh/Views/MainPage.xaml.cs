using System;
using System.ComponentModel;
using Radeoh.Services.RadeohService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Radeoh.Views
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            var service = new RadeohApiService();
            
            service.GetStations();
        }
    }
}
