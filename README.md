# Crypto Dotnet Ticker

A website that provides crypto price information. In this workshop we will:

- Implement a function in Azure Functions that runs only when data changes in an Azure Cosmos DB collection.
- Implement a function in Azure Functions to broadcast changes to connected clients using SignalR Service.
- Update the client web application to respond to SignalR messages.
- Use Azure Storage to host our Blazor site.


![Infrastructure](./assets/polling-to-signalr.png)
## Architecture

### Starter

The starter project consists of a Blazor WASM project that when loaded pulls data from an API. The data will only update when the website is reloaded.

### Completed 

The completed project consists of a Blazor WASM project that loads data from an API and this data is then reloaded in realtime whenever new data is available in the Cosmos DB collection. 

## Steps

1. Create a SignalR service account.
2. Create a Cosmos db account, database, and collection.
3. Create a Timer function that runs every 60 seconds, grabs data from API and sends to the Cosmos DB collection and to the SignalR Service hub. 
4. Create a HTTP function that will negotiate all connection to the SignalR hub. 
5. In the Blazor WASM app, add code that will connect to the SignalR service.
6. In the Blazor WASM app, add code that loads new data from SignalR service message.
7. In Blazor WASM app, add code that initial loads from HTTP function, subsequent loads should be real time.

## How to run locally
We will be developing with .NET 5. Please make sure you have [SDK installed](https://dotnet.microsoft.com/download)
Follow the instructions from [this documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp) to setup your local environnement

## SignalR and persistent connections

In contrast to polling, a more favorable design features persistent connections between the client and server. Establishing a persistent connection allows the server to push data to the client at will. The on-demand nature of the connection reduces network traffic and load on the server. SignalR allows you to easily add this type of architecture to your application.

SignalR is an abstraction for a series of technologies that allows your app to enjoy two-way communication between the client and server. SignalR handles connection management automatically, and lets you broadcast messages to all connected clients simultaneously, like a chat room. You can also send messages to specific clients. The connection between the client and server is persistent, unlike a classic HTTP connection, which is re-established for each communication.

A key benefit of the abstraction provided by SignalR is the way it supports "transport" fallbacks. A transport is method of communicating between the client and server. SignalR connections begin with a standard HTTP request. As the server evaluates the connection, the most appropriate communication method (transport) is selected. Transports are chosen depending on the APIs available on the client.

For clients that support HTML 5, the WebSockets API transport is used by default. If the client doesn't support WebSockets, then SignalR falls back to Server Sent Events (also known as EventSource). For older clients, Ajax long polling or Forever Frame (IE only) is used to mimic a two-way connection.

The abstraction layer offered by SignalR provides two benefits to your application. The first advantage is future-proofing your app. As the web evolves and APIs superior to WebSockets become available, your application doesn't need to change. You could update to a version of SignalR that supports any new APIs and your application code won't need an overhaul.

The second benefit is that SignalR allows your application to gracefully degrade depending on supported technologies of the client. If it doesn't support WebSockets, then Server Sent Events are used. If the client can't handle Server Sent Events, then it uses Ajax long polling, and so on.

Let's look at how to use SignalR to broadcast information from function that reads the Azure Cosmos DB change feed.

## Host the website

We use Azure storage to host our blazor website. Azure Storage includes a feature where you can place files in a specific storage container, which makes them available for HTTP requests. This feature, known as static website support makes hosting publicly available web pages a simple process.