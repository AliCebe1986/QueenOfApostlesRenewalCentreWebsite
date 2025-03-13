using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity to use ApplicationUser and add role support
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and default admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Roles to create
        string[] roleNames = { "Admin", "Staff", "User" };

        foreach (var role in roleNames)
        {
            var roleExists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
            if (!roleExists)
            {
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
        }

        // Create a default admin user if not exists
        string adminEmail = "admin@example.com";
        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin",
                Surname = "User"
            };
            var result = userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
        }
    }
    catch (Exception ex)
    {
        // Log the error (you could write to a file or use a logging framework)
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map area routes before the default route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Map the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

app.Run();
