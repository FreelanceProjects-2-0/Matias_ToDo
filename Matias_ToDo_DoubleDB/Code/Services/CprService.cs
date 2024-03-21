using Matias_ToDo_DoubleDB.Data;
using Matias_ToDo_DoubleDB.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Matias_ToDo_DoubleDB.Code.Services;

public class CprService : ICprService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly IRoleService _roleService;
    private readonly IHashingService _hashingService;
    private readonly DataDBContext _dataDbContext;
    private readonly ILogger _logger;
    UserManager<ApplicationUser> _userManager;
    IServiceProvider _serviceProvider;

    public CprService(ApplicationDbContext appDbContext, DataDBContext dataDbContext, IRoleService roleService, ILogger<CprService> logger, IServiceProvider serviceProvider, IHashingService hashingService)
    {
        _appDbContext = appDbContext;
        _dataDbContext = dataDbContext;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleService = roleService;
        _hashingService = hashingService;
    }

    /// <summary>
    /// CPR will be hashed in this function
    /// </summary>
    /// <param name="cpr"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> AddCprToUser(string cpr, string userEmail)
    {
        try
        {
            bool exists = await IsExisting(userEmail);
            var hashedCpr = _hashingService.BCryptHashing(cpr);
            if (!exists)
            {
                ApplicationUser? appUser = await _userManager.FindByEmailAsync(userEmail.ToUpper());
                if (appUser != null)
                {
                    Cpr cprRecord = new Cpr { UserMail = appUser.Email!.ToLower(), IdentityId = Guid.Parse(appUser.Id), CprNumber = hashedCpr };
                    _dataDbContext.Cprs.Add(cprRecord);
                    var response = await _roleService.CreateUserRole("CPR", _serviceProvider, userEmail);
                    _logger.LogInformation($"Response 2: {response}");
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
        var dbCprNumber = (await _dataDbContext.Cprs.FirstOrDefaultAsync(x => x.UserMail.ToUpper() == email.ToUpper()))!.CprNumber;
        return _hashingService.BCryptHashValidator(cpr, dbCprNumber);
    }

    public async Task<bool> Save()
    {
        return await _dataDbContext.SaveChangesAsync() > 0;
    }
}