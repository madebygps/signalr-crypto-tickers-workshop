using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class GetPricesJson
    {
        [Function("GetPricesJson")]
        public static object Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [CosmosDBInput("%DatabaseName%", "%CollectionName%", ConnectionStringSetting = "CosmosDBConnectionString")] Coin[] coins,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetPricesJson");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return coins;
        }
    }
}
