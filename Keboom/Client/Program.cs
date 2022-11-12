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

builder.Services.AddSingleton<MockClientSideGameHandler>();
builder.Services.AddSingleton<IGameHubEventHandler>(x=>x.GetRequiredService<MockClientSideGameHandler>());
builder.Services.AddSingleton<IGameHubClientSideMethods>(x => x.GetRequiredService<MockClientSideGameHandler>());

builder.Services.AddSingleton(x=>new GameHubConnection(x.GetRequiredService<NavigationManager>().ToAbsoluteUri("/gamehub").ToString(),
                                                        x.GetRequiredService<IGameHubClientSideMethods>(), x.GetRequiredService<IGameHubEventHandler>()));

var app = builder.Build();

//force the starting of the hub
app.Services.GetRequiredService<GameHubConnection>();

await app.RunAsync();
