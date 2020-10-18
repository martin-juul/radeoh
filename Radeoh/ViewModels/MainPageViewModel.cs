using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Radeoh.Models;
using Xamarin.Forms;

namespace Radeoh.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Station> Stations { get; set; }
        public ICommand FetchStationsCommand { get; set; }

        public MainPageViewModel()
        {
            FetchStationsCommand = new Command(async () => await RunSafe(FetchStations()));
        }
        async Task FetchStations()
        {
            var httpResponse = await ApiManager.GetStations();

            if (httpResponse.IsSuccessStatusCode)
            {
                var res = await httpResponse.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<Station>>(res));
                Stations = new ObservableCollection<Station>(json);
            }
            else
            {
                await PageDialog.AlertAsync("Unable to fetch stations", "Error", "Ok");
            }
        }
    }
}