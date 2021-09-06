using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace CryptoTicker.Pages
{
    public class FetechDataBase : ComponentBase
    {
        protected Coin[] prices;

        [Inject]
        public HttpClient Http {get;set;}
        protected override async Task OnInitializedAsync()
        {
            prices = await Http.GetFromJsonAsync<Coin[]>("https://cryptodatadotnet.azurewebsites.net/api/GetPricesJson");
        }
    }
}