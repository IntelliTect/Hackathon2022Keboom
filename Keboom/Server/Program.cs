using System.Text.Json.Serialization;
using Keboom.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
                .AddSignalR(
                    hubOptions =>
                    {
                        hubOptions.EnableDetailedErrors = true;
                    }
                )
                .AddJsonProtocol(
                    options =>
                    {
                        options.PayloadSerializerOptions.PropertyNamingPolicy =
                            System.Text.Json.JsonNamingPolicy.CamelCase;
                        options.PayloadSerializerOptions.ReferenceHandler =
                            ReferenceHandler.IgnoreCycles;
                    }
                ).AddHubOptions<GameHub>(
        options =>
        { //optional options
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
        }
    );

builder.Services.AddSingleton<IGameStore, GameStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();

app.MapHub<GameHub>($"/{nameof(GameHub)}");

app.MapFallbackToFile("index.html");

app.Run();
