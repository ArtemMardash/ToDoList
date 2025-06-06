using AuthService.Core.Entities;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Persistence.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly UserManager<AppUserDb> _userManager;

    public AppUserRepository(UserManager<AppUserDb> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Method that creates appUser
    /// </summary>
    public async Task<Guid> CreateUserAsync(AppUser user, CancellationToken cancellationToken)
    {
        var userDb = AppUserToAppUserDb(user);
        var result = await _userManager.CreateAsync(userDb, user.Password);
        if (result.Succeeded)
        {
            return Guid.Parse(userDb.Id);
        }

        throw new InvalidOperationException(string.Join("\n", result.Errors.Select(e => e.Description)));
    }

    /// <summary>
    /// Method to get appUser by id
    /// </summary>
    public async Task<AppUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var appUserDb = await _userManager.FindByIdAsync(id.ToString());

        if (appUserDb == null)
        {
            throw new ArgumentNullException(nameof(appUserDb), "There is no appUser with such ID");
        }

        return AppUserDbToAppUser(appUserDb);
    }

    /// <summary>
    /// Method to get appUser by email, mainly for login
    /// </summary>
    public async Task<AppUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var appUserDb = await _userManager.FindByEmailAsync(email);

        if (appUserDb == null)
        {
            throw new InvalidOperationException("There is no appUser with such email");
        }

        return AppUserDbToAppUser(appUserDb);
    }

    /// <summary>
    /// Method to validate password
    /// </summary>
    public async Task<bool> ValidatePasswordAsync(Guid id, string password, CancellationToken cancellationToken)
    {
        var appUserDb = await _userManager.FindByIdAsync(id.ToString());
        if (appUserDb == null)
        {
            throw new InvalidOperationException("There is no user with such Id");
        }

        return await _userManager.CheckPasswordAsync(appUserDb, password);
    }

    /// <summary>
    /// Method to check token
    /// </summary>
    public async Task<bool> CheckTokenAsync(Guid id, string currentRefreshToken, CancellationToken cancellationToken)
    {
        var appUserDb = await _userManager.FindByIdAsync(id.ToString());
        if (appUserDb == null || appUserDb.RefreshToken != currentRefreshToken ||
            appUserDb.RefreshTokenExpiry <= DateTime.Now)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method to set a new refresh token
    /// </summary>
    public async Task SetRefreshTokenAsync(Guid id, string refreshToken, DateTime tokenExpirationDate,
        CancellationToken cancellationToken)
    {
        var appUserDb = await _userManager.FindByIdAsync(id.ToString());
        if (appUserDb == null)
        {
            throw new InvalidOperationException("There is no user with such id");
        }

        appUserDb.RefreshToken = refreshToken;
        appUserDb.RefreshTokenExpiry = tokenExpirationDate;

        await _userManager.UpdateAsync(appUserDb);
    }

    /// <summary>
    /// Method to parse domain to db
    /// </summary>
    private AppUserDb AppUserToAppUserDb(AppUser user)
    {
        return new AppUserDb
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            UserName = user.Email,
            FullName = user.FullName.FirstName + " " + user.FullName.LastName
        };
    }

    /// <summary>
    /// Method to parse db to domain
    /// </summary>
    private AppUser AppUserDbToAppUser(AppUserDb appUserDb)
    {
        var firstName = appUserDb.FullName.Split(" ")[0];
        var lastName = appUserDb.FullName.Split(" ")[1];

        return new AppUser(Guid.Parse(appUserDb.Id), appUserDb.Email, appUserDb.PasswordHash,
            new FullName(firstName, lastName));
    }
}