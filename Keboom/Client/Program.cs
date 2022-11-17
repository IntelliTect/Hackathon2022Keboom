using Keboom.Client;
using Keboom.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ViewModels
builder.Services.AddTransient<GameBoardViewModel>();
builder.Services.AddTransient<ScoreBoardViewModel>();
builder.Services.AddTransient<MinefieldViewModel>();
builder.Services.AddTransient<IndexViewModel>();
builder.Services.AddTransient<GameOverViewModel>();

builder.Services.AddSingleton(x=>new GameHubConnection(x.GetRequiredService<NavigationManager>().ToAbsoluteUri("/gamehub").ToString()));

builder.Services.AddSingleton<IGameHubEventHandler>(x => x.GetRequiredService<GameHubConnection>());
builder.Services.AddSingleton<IGameHubServerSideMethods>(x => x.GetRequiredService<GameHubConnection>());
builder.Services.AddSingleton<IGameHubClientSideMethods>(x => x.GetRequiredService<GameHubConnection>());

var app = builder.Build();

//force the starting of the hub
_ = app.Services.GetRequiredService<GameHubConnection>().Open();

await app.RunAsync();
