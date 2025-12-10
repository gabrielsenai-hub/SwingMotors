using Microsoft.AspNetCore.Identity;

namespace AvaliacaoFinalWestn.Data;

public class Seed
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
       
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

}