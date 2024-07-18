using Fanex.Aff12B.Gateway.Host;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
// Add config to the container
builder.Configuration.SetBasePath(env.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var envFromSetting = builder.Configuration.GetValue<string>("AppSettings:Environment");
builder.Configuration.AddJsonFile($"App_Data/Ocelot/Ocelot.{envFromSetting}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"App_Data/Whitelist/Whitelist.{envFromSetting}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.UseSentry();

// Add services to the container.
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
app.MapGet("/", () => "Hello, Welcome to 12B API Gateway!");

//Configure the HTTP request pipeline.
await startup.Configure(app, app.Environment);

app.Run();