using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MassageContext")
    ?? throw new InvalidOperationException("Connection string 'MassageContext' not found.");

builder.Services.AddDbContext<MassageContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MassageContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ✅ Swagger (za API dokumentacijo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// (tvoj admin seed naj ostane kot imaš)

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Swagger UI (jaz priporočam samo v Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// ✅ NUJNO: mapiranje API controllerjev (attribute routes)
app.MapControllers();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
