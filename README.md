# Serverless Crypto Tickers

A static website with a serverless backend that provides crypto price information. The tickers update every in realtime whenever new data is made available to the database.

This project contains two different architectures and was made to demo how to implement real-time functionality to a static site.

View it live [here](https://cryptotickerstaticsite.z20.web.core.windows.net/)

![Infrastructure](./assets/polling-to-signalr.png)
## Architecture

### Starter

The starter project consists of a [Blazor WASM](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) static website [deployed on Azure Storage](https://microsoft.github.io/AzureTipsAndTricks/blog/tip221.html). The website will make a GET call to [Coin Gecko API](https://www.coingecko.com/en/api) and display its information. 

The data will only update when the website is reloaded/refreshed.

### Completed 

The starter project consists of a [Blazor WASM](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) static website [deployed on Azure Storage](https://microsoft.github.io/AzureTipsAndTricks/blog/tip221.html). The website will make a GET call to  an API deployed on [Azure Functions](https://learn.microsoft.com/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=in-process%2Cfunctionsv2&pivots=programming-language-csharp) and display its information. 

Subsequent new data is pushed in realtime to the static website without reloading, whenever new data is available in the [Cosmos DB](https://azure.microsoft.com/products/cosmos-db/) collection via Azure Functions and [SignalR Service](https://learn.microsoft.com/azure/azure-signalr/signalr-overview).

## Steps to go from starter to completed.

You can clone the code and the completed project will work once you populate the API's `local.settings.json` with you resources. However if you want to take the starter project and implement real-time functionality on your own to learn and get hands-on, here are some guidelines:

### API

0. Get a [Coin Gecko API key](https://www.coingecko.com/en/api)
1. Create necessary Azure Resources:
    - Resource Group
    - Cosmos DB account (my project uses NoSQL API but you can adapt to any)
        - Cosmo DB collection and Database
    - SignalR Service
2. Create a class that will describe your object. Mine is `Shared/Coin.cs`
3. Create an Azure Function with TimerTrigger. I set mine to 60 seconds but you can adjust the occurrence to whatever. Mine is `GetCryptoPrices.cs` 
    - Populate your `local.settings.json` with appropriate connection strings, collection names, etc. 
    - This Function should make Coin Gecko GET API call, send data to Cosmos DB collection and to the SignalR service hub.
4. Create an Azure Function with HTTPTrigger that will act as the SignalR negotiate. Mine is `negotiate.cs`
5. Create an Azure Function with HTTPTrigger that will connect to your Cosmos DB Database and Collection and return the data in said collection as json. This you can use for first time data loading in Client. Mine is `GetPricesJson.cs`

### Client

1. Create a razor component that will display your Tickers. Mine is `Pages/Tickers.razor`
2. My project has the `Pages/fetchdata.razor` component load the `Pages/Ticker.razor` component so my logic is in `fetchdata.razor.cs`. You'll have to add code that will:
    - load the json from your HTTP Function `GetJsonPrices.cs` for first time data load.
    - Connect to the SignalR Service hub
    - Loads new data from SignalR Service hub. I've done this in `fetchdata.razor.cs` component.

#### Client UI

I'm using [MudBlazor](https://mudblazor.com/) for Client UI components, it's awesome. 


## How to run locally

### Install

- [.NET 6](https://dotnet.microsoft.com/download)
- All the pre requisites from [this documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp)

### Debug

I've created custom [VSCode Launch.json](https://code.visualstudio.com/docs/editor/debugging) tasks. Make sure the API is running before you run the Client.

- Use `Attach to .NET Functions` to debug the API.
- Use `(Starter) Launch and Debug Standalone Blazor WebAssembly App` to debug the starter Client project.
- Use `(Completed) Launch and Debug Standalone Blazor WebAssembly App` to debug the Completed Client project.

## Known issues

- I've included a `devcontainer.json` for codespaces and devcontainer support, however it's not fully working quite yet. WIP.
- CI/CD is coming. At the moment the client is deployed on Azure Storage Account. 

## Additional resources

This demo is a spin on this learning module, check it out for more hands-on fun! https://aka.ms/UpdateWebApp6
