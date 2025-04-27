using AuthService.Core.Entities;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Persistence.Repositories;

public class AppUserRepository: IAppUserRepository
{
    private readonly UserManager<AppUserDb> _userManager;

    public AppUserRepository(UserManager<AppUserDb> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Guid> CreateUserAsync(AppUser user, CancellationToken cancellationToken)
    {
        var userDb = AppUserToAppUserDb(user);
        await _userManager.CreateAsync(userDb, user.Password);
        return Guid.Parse(userDb.Id);
    }

    public async Task<AppUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var appUserDb = await  _userManager.FindByIdAsync(id.ToString());
        
        if (appUserDb == null)
        {
            throw new ArgumentNullException(nameof(appUserDb), "There is no appUser with such ID");
        }

        return AppUserDbToAppUser(appUserDb);
    }

    public async Task<AppUser> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var appUserDb = await _userManager.FindByEmailAsync(email);
        
        if (appUserDb == null)
        {
            throw new ArgumentNullException(nameof(appUserDb), "There is no appUser with such email");
        }

        return AppUserDbToAppUser(appUserDb);
    }

    public Task<bool> ValidatePasswordAsync(AppUser appUser, string password, CancellationToken cancellationToken)
    {
        return Task.FromResult(appUser.Password == password);
    }

    private AppUserDb AppUserToAppUserDb(AppUser user)
    {
        return new AppUserDb
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            FullName = user.FullName.FirstName + " " + user.FullName.LastName,
        };
    }

    private AppUser AppUserDbToAppUser(AppUserDb appUserDb)
    {
        var firstName = appUserDb.FullName.Split(" ")[0];
        var lastName = appUserDb.FullName.Split(" ")[1];

        return new AppUser(Guid.Parse(appUserDb.Id), appUserDb.Email, appUserDb.PasswordHash,
            new FullName(firstName, lastName));
    }
}