using ActivityManagementApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ActivityManagementApp.Services
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(AuthenticationStateProvider authenticationStateProvider, UserManager<ApplicationUser> userManager)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _userManager = userManager;
        }

        /// <summary>
        /// 現在ログインしているユーザーの ID（UserId）を取得
        /// </summary>
        public async Task<string?> GetUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                return user.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return null;
        }

        /// <summary>
        /// 現在のユーザーがログイン中か判定
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// メールアドレスからユーザIDを取得
        /// </summary>
        public async Task<string?> GetUserIdByEmailAsync(string? email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            var user = await _userManager.FindByEmailAsync(email);
            return user?.Id;
        }
    }
}
