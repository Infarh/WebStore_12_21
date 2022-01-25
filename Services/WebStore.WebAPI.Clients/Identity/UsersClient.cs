using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Clients.Base;
// ReSharper disable AsyncApostle.AsyncAwaitMayBeElidedHighlighting

namespace WebStore.WebAPI.Clients.Identity;

public class UsersClient : BaseClient, IUsersClient
{
    public UsersClient(HttpClient Client) : base(Client, WebAPIAddresses.Identity.Users)
    {
    }

    #region Implementation of IUserStore<User>

    public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/UserId", user, cancel);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/UserName", user, cancel);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task SetUserNameAsync(User user, string name, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/UserName/{name}", user, cancel);
        user.UserName = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/NormalUserName/", user, cancel);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/NormalUserName/{name}", user, cancel);
        user.NormalizedUserName = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/User", user, cancel);
        var creation_success = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);

        return creation_success
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
    {
        var response = await PutAsync($"{Address}/User", user, cancel);
        var update_result = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);

        return update_result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/User/Delete", user, cancel);
        var delete_result = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);

        return delete_result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<User> FindByIdAsync(string id, CancellationToken cancel)
    {
        return await GetAsync<User>($"{Address}/User/Find/{id}", cancel).ConfigureAwait(false);
    }

    public async Task<User> FindByNameAsync(string name, CancellationToken cancel)
    {
        return await GetAsync<User>($"{Address}/User/Normal/{name}", cancel).ConfigureAwait(false);
    }

    #endregion

    #region Implementation of IUserRoleStore<User>

    public async Task AddToRoleAsync(User user, string role, CancellationToken cancel)
    {
        await PostAsync($"{Address}/Role/{role}", user, cancel).ConfigureAwait(false);
    }

    public async Task RemoveFromRoleAsync(User user, string role, CancellationToken cancel)
    {
        await PostAsync($"{Address}/Role/Delete/{role}", user, cancel).ConfigureAwait(false);
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/roles", user, cancel).ConfigureAwait(false);
        return (await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<IList<string>>(cancellationToken: cancel)
           .ConfigureAwait(false))!;
    }

    public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/InRole/{role}", user, cancel);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken cancel)
    {
        return (await GetAsync<List<User>>($"{Address}/UsersInRole/{role}", cancel).ConfigureAwait(false))!;
    }

    #endregion

    #region Implementation of IUserPasswordStore<User>

    public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetPasswordHash", new PasswordHashDTO { User = user, Hash = hash }, cancel)
           .ConfigureAwait(false);
        user.PasswordHash = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel);
        //user.PasswordHash = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
    }

    public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetPasswordHash", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/HasPassword", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    #endregion

    #region Implementation of IUserEmailStore<User>

    public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetEmail/{email}", user, cancel).ConfigureAwait(false);
        user.Email = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetEmail", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetEmailConfirmed", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetEmailConfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
        user.EmailConfirmed = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<User> FindByEmailAsync(string email, CancellationToken cancel)
    {
        return (await GetAsync<User>($"{Address}/User/FindByEmail/{email}", cancel).ConfigureAwait(false))!;
    }

    public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/User/GetNormalizedEmail", user, cancel);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetNormalizedEmail/{email}", user, cancel).ConfigureAwait(false);
        user.NormalizedEmail = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    #endregion

    #region Implementation of IUserPhoneNumberStore<User>

    public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetPhoneNumber/{phone}", user, cancel).ConfigureAwait(false);
        user.PhoneNumber = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetPhoneNumber", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetPhoneNumberConfirmed", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetPhoneNumberConfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
        user.PhoneNumberConfirmed = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    #endregion

    #region Implementation of IUserLoginStore<User>

    public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
    {
        await PostAsync($"{Address}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancel).ConfigureAwait(false);
    }

    public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken cancel)
    {
        await PostAsync($"{Address}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel).ConfigureAwait(false);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetLogins", user, cancel).ConfigureAwait(false);
        return (await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken: cancel)
           .ConfigureAwait(false))!;
    }

    public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
    {
        return (await GetAsync<User>($"{Address}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancel).ConfigureAwait(false))!;
    }

    #endregion

    #region Implementation of IUserLockoutStore<User>

    public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetLockoutEndDate", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? EndDate, CancellationToken cancel)
    {
        var response = await PostAsync(
                $"{Address}/SetLockoutEndDate",
                new SetLockoutDTO { User = user, LockoutEnd = EndDate },
                cancel)
           .ConfigureAwait(false);
        user.LockoutEnd = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/IncrementAccessFailedCount", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<int>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel)
    {
        await PostAsync($"{Address}/ResetAccessFailedCont", user, cancel).ConfigureAwait(false);
    }

    public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetAccessFailedCount", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<int>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetLockoutEnabled", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetLockoutEnabled/{enabled}", user, cancel).ConfigureAwait(false);
        user.LockoutEnabled = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    #endregion

    #region Implementation of IUserTwoFactorStore<User>

    public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/SetTwoFactor/{enabled}", user, cancel).ConfigureAwait(false);
        user.TwoFactorEnabled = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetTwoFactorEnabled", user, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    #endregion

    #region Implementation of IUserClaimStore<User>

    public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetClaims", user, cancel).ConfigureAwait(false);
        return (await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<List<Claim>>(cancellationToken: cancel)
           .ConfigureAwait(false))!;
    }

    public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
    {
        await PostAsync(
                $"{Address}/AddClaims",
                new AddClaimDTO { User = user, Claims = claims },
                cancel)
           .ConfigureAwait(false);
    }

    public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
    {
        await PostAsync(
                $"{Address}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, Claim = OldClaim, NewClaim = NewClaim },
                cancel)
           .ConfigureAwait(false);
    }

    public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
    {
        await PostAsync(
                $"{Address}/RemoveClaims",
                new RemoveClaimDTO { User = user, Claims = claims },
                cancel)
           .ConfigureAwait(false);
    }

    public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
    {
        var response = await PostAsync($"{Address}/GetUsersForClaim", claim, cancel).ConfigureAwait(false);
        return (await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<List<User>>(cancellationToken: cancel)
           .ConfigureAwait(false))!;
    }

    #endregion
}