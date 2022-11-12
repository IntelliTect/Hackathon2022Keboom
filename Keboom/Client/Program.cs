using Keboom.Client;
using Keboom.Client.ViewModels;
using Keboom.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ViewModels
builder.Services.AddTransient<GameBoardViewModel>();
builder.Services.AddTransient<ScoreBoardViewModel>();

builder.Services.AddSingleton(x=>new GameHubConnection(x.GetRequiredService<NavigationManager>().ToAbsoluteUri("/gamehub").ToString()));

builder.Services.AddSingleton<IGameHubEventHandler>(x => x.GetRequiredService<GameHubConnection>());
builder.Services.AddSingleton<IGameHubServerSideMethods>(x => x.GetRequiredService<GameHubConnection>());
builder.Services.AddSingleton<IGameHubClientSideMethods>(x => x.GetRequiredService<GameHubConnection>());

var app = builder.Build();

//force the starting of the hub
app.Services.GetRequiredService<GameHubConnection>();

await app.RunAsync();
