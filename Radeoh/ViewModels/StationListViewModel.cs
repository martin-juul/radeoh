using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Radeoh.Models;
using Xamarin.Forms;

namespace Radeoh.ViewModels
{
    public class StationListViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Station> _stations;
        public ICommand FetchStationsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public StationListViewModel()
        {
            FetchStationsCommand = new Command(async () => await RunSafe(FetchStations()));
        }

        public ObservableCollection<Station> Stations
        {
            get => _stations;
            set
            {
                _stations = value;
                OnPropertyChanged();
            }
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }
    }
}