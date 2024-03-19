using Matias_ToDo_DoubleDB.Data;
using Microsoft.AspNetCore.Identity;

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

        //if (roleResponse != null && roleResponse.Succeeded)
        //{
        //    if (userResponse != null && userResponse.Succeeded)
        //    {
        //        return true;
        //    }
        //    Console.WriteLine($"userResponse");
        //    return false;
        //}

        //return false;
    }
}

