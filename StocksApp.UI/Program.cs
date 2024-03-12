using Serilog;
using StocksApp.Middleware;
using StocksApp.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Test"))
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(app.Services)
    .WriteTo.Console()
    .CreateLogger();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandlingMiddleware();
}

if (!builder.Environment.IsEnvironment("Test"))
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }
