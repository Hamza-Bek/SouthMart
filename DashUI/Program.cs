using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Blazored.LocalStorage;
using DashUI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static System.Net.Mime.MediaTypeNames;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<Application.Extensions.LocalStorageService>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SellerOnly", policy => policy.RequireRole("Seller"));
});



builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<CustomHttpHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();

#region Implementation
builder.Services.AddScoped<IAccountRepository, AccountService>();
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<IBuyerRepository, BuyerService>();
builder.Services.AddScoped<IOrderRepository, OrderService>();
builder.Services.AddScoped<ICommentRepository, CommentService>();
builder.Services.AddScoped<ILocationRepository, LocationService>();
builder.Services.AddScoped<IMenuRepository, MenuService>();
builder.Services.AddScoped<IFilesRepository, FilesService>();
builder.Services.AddScoped<ISellerRepository, SellerService>();
#endregion

#region HTTP's CLients
builder.Services.AddHttpClient<ISellerRepository, SellerService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<IFilesRepository, FilesService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<IMenuRepository, MenuService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<ILocationRepository, LocationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<ICommentRepository, CommentService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
builder.Services.AddHttpClient<IOrderRepository, OrderService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7207/");
});
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
#endregion

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7207") });

await builder.Build().RunAsync();
