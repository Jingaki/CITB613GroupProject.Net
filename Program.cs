using DotNetEd.CoreAdmin;
using GroupProjectDepositCatalogWebApp.Data;
using GroupProjectDepositCatalogWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>() // Add roles to the Identity configuration
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddCoreAdmin(new CoreAdminOptions()
{
    IgnoreEntityTypes = new List<Type>() {
        typeof(DepositModel),
        typeof(ApplicationUserModel)
    },
    RestrictToRoles = new List<string>() {
        "Admin of the website"
    }.ToArray()
});
//builder.Services.AddCoreAdmin();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
// example on clean up https://www.moitepari.bg/depoziti/razplashtatelen-plan.aspx?s=rDBk-eDVjboJLTy115*hS9RLcCNdz6sH8MLX-b)tU4TFkggY09JcyPaZ7D222x68sYMybbg!gBT03SlT!D(o1V*v6T1fpl)wa_BawEMNuWKLlskXj*csFugD2d