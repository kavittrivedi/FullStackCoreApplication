using FullStackApplication.AppSettingsModels;
using FullStackApplication.Services;
using FullStackApplication.Services.Contract;
using FullStackApplication.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Retrieve API settings to configure HttpClient
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
// Register HttpClient
//builder.Services.AddHttpClient<ApiService>();
//builder.Services.AddHttpClient<ApiService>(client =>
//{
//    client.BaseAddress = new Uri(apiSettings.BaseUrl);
//});

// Register HttpClient with base address
builder.Services.AddHttpClient<ApiService>(client =>
{
    var url = new Uri(apiSettings.BaseUrl);// Replace with your API base URL
    client.BaseAddress = url; 
});



// Register ICategoryService and its implementation
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Configure JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<HttpClientService>();
/*builder.Services.AddScoped<ApiService>();*/  // Register ApiService

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session middleware
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
