using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ActivityManagementApp.Services
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
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
    }
}
