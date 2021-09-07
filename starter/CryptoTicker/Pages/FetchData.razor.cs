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
            prices = await Http.GetFromJsonAsync<Coin[]>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false");
        }
    }
}