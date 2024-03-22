using Matias_ToDo_DoubleDB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Matias_ToDo_DoubleDB.Code.Services;

public class RoleService : IRoleService
{
    public async Task<bool> CreateUserRole(string role, IServiceProvider serviceProvider, string userEmail)
    {
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        // Call the manager as we are not allowed to access Identity
        bool roleSuccess = false;
        bool userSuccess = false;


        if (!await roleManager.RoleExistsAsync(role.ToLower()))
        {
            IdentityResult roleResponse = await roleManager.CreateAsync(new IdentityRole(role));
            roleSuccess = roleResponse.Succeeded;
        }

        ApplicationUser? identityUser = await userManager.FindByEmailAsync(userEmail.ToUpper());

        if (identityUser != null)
        {
            IdentityResult userResponse = await userManager.AddToRoleAsync(identityUser, role);
            userSuccess = userResponse.Succeeded;
        }

        return roleSuccess | userSuccess;
    }

    public async Task<bool> RemoveUserRole(string role, IServiceProvider serviceProvider, string userEmail)
    {
        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        // Call the manager as we are not allowed to access Identity
        bool userSuccess = false;

        ApplicationUser? identityUser = await userManager.FindByEmailAsync(userEmail.ToUpper());

        if (identityUser != null)
        {
            IdentityResult userResponse = await userManager.RemoveFromRoleAsync(identityUser, role);
            userSuccess = userResponse.Succeeded;
        }

        return userSuccess;
    }

    public async Task<bool> IsUserAdmin(string userEmail, IServiceProvider serviceProvider)
    {
        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.Users.Where(x => x.NormalizedEmail == userEmail.ToUpper()).FirstOrDefaultAsync();
        if (user == null) throw new Exception($"Something went wrong while trying to fetch user from DB");
        return await userManager.IsInRoleAsync(user, "Admin");
    }
}

