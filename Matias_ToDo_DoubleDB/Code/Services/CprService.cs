using Matias_ToDo_DoubleDB.Data;
using Matias_ToDo_DoubleDB.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Matias_ToDo_DoubleDB.Code.Services;

public class CprService : ICprService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly DataDBContext _dataDbContext;
    private readonly ILogger _logger;
    UserManager<ApplicationUser> _userManager;

    public CprService(ApplicationDbContext appDbContext, DataDBContext dataDbContext, ILogger<CprService> logger, IServiceProvider serviceProvider)
    {
        _appDbContext = appDbContext;
        _dataDbContext = dataDbContext;
        _logger = logger;
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    }

    public async Task<bool> AddCprToUser(string cpr, string userEmail)
    {
        try
        {
            bool exists = await IsExisting(userEmail);
            if (!exists)
            {
                ApplicationUser? appUser = await _userManager.FindByEmailAsync(userEmail.ToUpper());
                if (appUser != null)
                {
                    Cpr cprRecord = new Cpr { UserMail = appUser.Email!.ToLower(), IdentityId = Guid.Parse(appUser.Id), CprNumber = cpr };
                    _dataDbContext.Cprs.Add(cprRecord);
                    return await Save();
                }
            }
            else if (await CheckCprAsync(cpr, userEmail))
            {
                _logger.LogInformation($"Davs");
                return true;
            }
            return false;
        }
        catch (Exception err)
        {
            throw new Exception($"{userEmail}", err);
        }
    }

    private async Task<bool> IsExisting(string email)
    {
        return await _dataDbContext.Cprs.AnyAsync(x => x.UserMail.ToLower() == email.ToLower());
    }

    private async Task<bool> CheckCprAsync(string cpr, string email)
    {
        return await _dataDbContext.Cprs.AnyAsync(x => x.UserMail.ToLower() == email.ToLower() && x.CprNumber == cpr);
    }

    public async Task<bool> Save()
    {
        var a = await _dataDbContext.SaveChangesAsync();
        _logger.LogDebug($"{a}");
        return a > 0;
    }
}