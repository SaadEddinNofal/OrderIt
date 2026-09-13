using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Repository;
using OrderITDemo.Repository.Base;
using OrderITDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderITDemo.Abstraction;
/*using Quartz;
using Quartz.Impl;*/
//using OrderITDemo.Services;

var builder = WebApplication.CreateBuilder(args);
/*var emailSettings = builder.Configuration.GetSection("EmailSettings");
var smtpServer = emailSettings["SmtpServer"];
var port = int.Parse(emailSettings["Port"]);*/

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    try
    {
        var diag = new System.Text.StringBuilder();
        var contentRoot = builder.Environment.ContentRootPath;
        var envName = builder.Environment.EnvironmentName;
        var settingsPath = System.IO.Path.Combine(contentRoot, "appsettings.json");
        var settingsExists = System.IO.File.Exists(settingsPath);
        var envVar = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        var currentDir = Directory.GetCurrentDirectory();

        diag.AppendLine("[OrderIt startup diagnostic]");
        diag.AppendLine($"Time          : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        diag.AppendLine($"Environment   : {envName}");
        diag.AppendLine($"ContentRoot   : {contentRoot}");
        diag.AppendLine($"CurrentDir    : {currentDir}");
        diag.AppendLine($"appsettings   : {settingsPath}");
        diag.AppendLine($"appsettings exists: {settingsExists}");
        diag.AppendLine($"env var 'ConnectionStrings__DefaultConnection' present: {(envVar is null ? "NO (null)" : envVar.Length == 0 ? "YES (empty)" : "YES (has value)")}");
diag.AppendLine("----- PROVIDERS (key 'ConnectionStrings:DefaultConnection'; value redacted) -----");
var root = (builder.Configuration as IConfigurationRoot);
if (root is not null)
{
    foreach (var provider in root.Providers)
    {
        var ok = provider.TryGet("ConnectionStrings:DefaultConnection", out _);
        diag.AppendLine($"  [{provider.GetType().Name}] contains key: {(ok ? "YES" : "no")}");
    }
}
        var diagPath = System.IO.Path.Combine(contentRoot, "startup-diag.txt");
        System.IO.File.WriteAllText(diagPath, diag.ToString());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[OrderIt] Failed to write startup diagnostic: {ex.Message}");
    }

    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is missing or empty. " +
        "Set the 'ConnectionStrings__DefaultConnection' environment variable on the server, " +
        "or add it to appsettings.json. A diagnostic file 'startup-diag.txt' was written next to the app — open it to see what the app actually loaded.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepository<>), typeof(MainRepository<>));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();
builder.Services.AddIdentity<AppUser, IdentityRole>(option => option.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 0;
});
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<BackGroundService>();    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseHangfireDashboard("/dash");
app.UseAuthorization();
    
app.MapStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
