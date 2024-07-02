using Fanex.Aff12B.Identity.Host;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
// Add config to the container
builder.Configuration.SetBasePath(env.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.UseSentry();

// Add services to the container.
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
app.MapGet("/", () => $"Hello, Welcome to internal 12B API Authentication!");

// Configure the HTTP request pipeline.
startup.Configure(app);

app.MapDefaultControllerRoute();

app.Run();