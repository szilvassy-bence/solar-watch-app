using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Data;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<AppUser> _userManager;
    private readonly ILogger<AuthenticationSeeder> _logger;

    public AuthenticationSeeder(
        RoleManager<IdentityRole> roleManager, 
        UserManager<AppUser> userManager, 
        ILogger<AuthenticationSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }
    
    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(_roleManager);
        tAdmin.Wait();

        var tUser = CreateUserRole(_roleManager);
        tUser.Wait();
    }
    
    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            _logger.LogInformation("New role created: Admin");
        }
    }

    async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
            _logger.LogInformation("New role created: User");
        }
    }


    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await _userManager.FindByEmailAsync("a@a");
        if (adminInDb == null)
        {
            var admin = new AppUser() { UserName = "admin", Email = "a@a" };
            var adminCreated = await _userManager.CreateAsync(admin, "admin123");
            if (adminCreated.Succeeded) await _userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}