using FullStackApplication.Services;
using FullStackApplication.Services.Contract;
using FullStackApplication.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllersWithViews();

// Register configuration settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Retrieve API settings to configure HttpClient
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});
// For https.
//var httpClientHandler = new HttpClientHandler();
//httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

//builder.Services.AddHttpClient<ApiService>(client =>
//{
//    client.BaseAddress = new Uri(apiSettings.BaseUrl);
//}).ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);

builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

