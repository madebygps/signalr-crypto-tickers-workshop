using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Completed.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Completed.Client.Pages
{
    public class FetechDataBase : ComponentBase
    {
        protected Coin[] prices;
        [Inject]
        public HttpClient Http {get;set;}
    protected HubConnection hubConnection;
    public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

    protected override async Task OnInitializedAsync()
    {
        

        // prices = await Http.GetFromJsonAsync<Coin[]>("https://cryptodatadotnet.azurewebsites.net/api/GetPricesJson");
        prices = await Http.GetFromJsonAsync<Coin[]>("http://localhost:7170/api/GetPricesJson");
            
        hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:7170/api").Build();

        hubConnection.On<Coin[]>("updated", (coin) =>
            {
            prices = coin;
                StateHasChanged();
            }                  
        );                                                                                          
   
        await hubConnection.StartAsync();
                 
    }

     public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
    }
}