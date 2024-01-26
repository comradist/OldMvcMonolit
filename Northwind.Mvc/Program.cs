using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Northwind.Mvc.Models;
using Northwind.Mvc.Data;
using Northwind.Mvc.Settigns;
using Northwind.Mvc.Service;
using Packt.Shared;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString1 = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var connectionString2 = builder.Configuration.GetConnectionString("ConnectionForFileDB") ?? throw new InvalidOperationException("Connection string 'ConnectionForFileDB' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString1));

builder.Services.AddDbContext<FileDbContext>(options => options.UseSqlite(connectionString2));

builder.Services.AddNortwindContext();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<UserExtendedForIdentity, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddHttpClient(name: "Northwind.WebApi", configureClient: option =>
{
    option.BaseAddress = new Uri("http://localhost:5002/");
    option.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
});

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(20);
    options.AddPolicy("views", p => p.SetVaryByQuery(""));
});


WebApplication app = builder.Build();
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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseOutputCache();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").CacheOutput("views");

app.MapRazorPages();

app.MapGet("/notcached", () => DateTime.Now.ToString());
app.MapGet("/cached", () => DateTime.Now.ToString()).CacheOutput();

app.Run();
