using Keboom.Client;
using Keboom.Client.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ViewModels
builder.Services.AddTransient<GameBoardViewModel>();

await builder.Build().RunAsync();
