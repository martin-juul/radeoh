using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Fusillade;
using Polly;
using Refit;
using Xamarin.Essentials;

namespace Radeoh.Services.HttpApi
{
    public class ApiManager : IApiManager
    {
        IUserDialogs _userDialogs = UserDialogs.Instance;
        IApiService<IRadeohApi> radeohApi;
        public bool IsConnected { get; set; }
        Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();

        Dictionary<string, Task<HttpResponseMessage>> taskContainer =
            new Dictionary<string, Task<HttpResponseMessage>>();

        public ApiManager(IApiService<IRadeohApi> radeohApi)
        {
            this.radeohApi = radeohApi;
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;

            if (!IsConnected)
            {
                // Cancel All Running Task
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        public async Task<HttpResponseMessage> GetStations()
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(radeohApi.GetApi(Priority.UserInitiated)
                .GetStations(cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }


        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = new TData();

            if (!IsConnected)
            {
                var stringResponse = "There's not a network connection";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(stringResponse);

                _userDialogs.Toast(stringResponse, TimeSpan.FromSeconds(1));
                return data;
            }

            data = await Policy
                .Handle<WebException>()
                .Or<ApiException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync
                (
                    retryCount: 1,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () =>
                {
                    var result = await task;

                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _userDialogs.Alert("Unauthorized");
                    }

                    runningTasks.Remove(task.Id);

                    return result;
                });

            return data;
        }
    }
}