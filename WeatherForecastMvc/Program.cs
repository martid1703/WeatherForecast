using Microsoft.EntityFrameworkCore;
using WeatherForecastMvc.HostedServices;

var builder = WebApplication.CreateBuilder(args);

string path = Path.Combine(Directory.GetCurrentDirectory(), "app_data");
var connString = builder.Configuration.GetConnectionString("ForecastContext").Replace("|DataDirectory|", path);

var dbContext = builder.Services.AddDbContext<ForecastContext>(options =>
    options.UseSqlServer(connString ?? throw new InvalidOperationException("Connection string 'ForecastContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// var forecastConfig = builder.Configuration.GetSection("WeatherForecastMvcConfig");
// builder.Services.Configure<TimedService>(forecastConfig);
builder.Services.AddHostedService<TimedService>();
//builder.Services.AddHostedService<HostedService>();
//builder.Services.AddScoped<IForecastCleanuperService, ForecastCleanuperService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Forecast/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Forecast}/{action=Index}/{id?}");

app.Run();
