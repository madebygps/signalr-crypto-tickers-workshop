using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class negotiate
    {
        [Function("negotiate")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "stocks")] string connectionInfo,
        
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("negotiate");
            logger.LogInformation("C# HTTP trigger function processed a request.");
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            response.WriteString(connectionInfo);

            return response;
        }
    }
    public class MyConnectionInfo
    {
        public string Url { get; set; }

        public string ConnectionId {get;set;}

        public string AccessToken { get; set; }
    }

}
