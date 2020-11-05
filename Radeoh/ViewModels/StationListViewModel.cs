using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
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
        private bool _hasStations;
        public ICommand FetchStationsCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public StationListViewModel()
        {
            if (_hasStations)
            {
                return;
            }
            FetchStationsCommand = new Command(async () => await RunSafe(FetchStations()));
        }

        public ObservableCollection<Station> Stations
        {
            get => _stations;
            private set
            {
                _stations = value;
                OnPropertyChanged();
            }
        }

        async Task FetchStations()
        {
            if (_hasStations) return;
            
            var httpResponse = await ApiManager.GetStations();

            if (httpResponse.IsSuccessStatusCode)
            {
                var res = await httpResponse.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<Station>>(res));
                Stations = new ObservableCollection<Station>(json);
                this._hasStations = true;
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