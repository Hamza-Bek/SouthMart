using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<Application.Extensions.LocalStorageService>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});



builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<CustomHttpHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IAccountRepository, AccountService>();
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<IBuyerRepository, BuyerService>();


builder.Services.AddHttpClient<IBuyerRepository, BuyerService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<IProductRepository, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<IAccountRepository, AccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});

builder.Services.AddHttpClient("WebUIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7207");
}).AddHttpMessageHandler<CustomHttpHandler>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7207") });

await builder.Build().RunAsync();
