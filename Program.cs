using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Preberi connection string iz appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MassageContext")
    ?? throw new InvalidOperationException("Connection string 'MassageContext' not found.");

// Registriraj EF Core DbContext z SQL Server (Azure SQL)
builder.Services.AddDbContext<MassageContext>(options =>
    options.UseSqlServer(connectionString));

// Identity z ApplicationUser + role + EF store na MassageContext
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MassageContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Error handling + HSTS v productionu
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Preusmeri "/" na /Home
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Home");
        return;
    }
    await next();
});

app.Run();
