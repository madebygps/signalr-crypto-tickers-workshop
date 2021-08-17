using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class GetCryptoPrices
    {
    
         
        [Function("GetCryptoPrices")]
        [CosmosDBOutput("%DatabaseName%", "%CollectionName%", ConnectionStringSetting = "CosmosDBConnectionString")]
        
        public async static Task<object> Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] MyInfo myTimer, FunctionContext context)
        {
     
            Coin[] prices;
            var logger = context.GetLogger("GetCryptoPrices");
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            
            using (var httpClient = new HttpClient())
            {
            
                var coinData = await httpClient.GetAsync("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=bitcoin%2C%20ethereum%2C%20cardano%2C%20dogecoin%2C%20usd-coin&order=market_cap_desc&per_page=100&page=1&sparkline=false");                
                var body = await coinData.Content.ReadAsStringAsync();
                prices = JsonSerializer.Deserialize<Coin[]>(body);              
                 
                foreach(var coin in prices)
                {
                    logger.LogInformation(coin.symbol);

                }
            }
            
            return prices;
        
        }
    }

    
    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
