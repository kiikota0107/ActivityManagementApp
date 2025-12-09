using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using ActivityManagementApp.Models.Api;

namespace ActivityManagementApp.Services
{
    public class DevicePairingService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserService _userService;
        private readonly TimeZoneService _timeZoneService;

        public DevicePairingService(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            UserService userService,
            TimeZoneService timeZoneService)
        {
            _contextFactory = contextFactory;
            _userService = userService;
            _timeZoneService = timeZoneService;
        }

        /// <summary>
        /// ランダムなペアリングコードを生成する（例：ABCD-1234）
        /// </summary>
        public string GeneratePairingCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string part1 = RandomString(4, chars);
            string part2 = RandomString(4, chars);

            return $"{part1}-{part2}";
        }

        private string RandomString(int length, string chars)
        {
            var data = RandomNumberGenerator.GetBytes(length);
            var sb = new StringBuilder(length);

            foreach (var b in data)
            {
                sb.Append(chars[b % chars.Length]);
            }

            return sb.ToString();
        }

        public async Task<DevicePairingCode> CreatePairingCodeAsync(string? deviceName = null)
        {
            var userId = await _userService.GetUserIdAsync()
                ?? throw new InvalidOperationException("ユーザーIDを取得できません。ログイン状態を確認してください。");
            var code = GeneratePairingCode();
            var now = _timeZoneService.NowJst;

            using var context = await _contextFactory.CreateDbContextAsync();

            var record = new DevicePairingCode
            {
                Code = code,
                UserId = userId,
                DeviceName = deviceName,
                CreatedAt = now,
                ExpiresAt = now.AddMinutes(10),
                IsUsed = false
            };

            context.DevicePairingCodes.Add(record);
            await context.SaveChangesAsync();

            return record;
        }

        public async Task<ActivateResult> ActivateAsync(ActivateRequest request)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var code = await context.DevicePairingCodes.FirstOrDefaultAsync(x => x.Code == request.Code);

            if (code == null) return ActivateResult.Fail("Invalid code.");

            if (code.ExpiresAt < _timeZoneService.NowJst)
                return ActivateResult.Fail("Code expired.");

            if (code.IsUsed)
                return ActivateResult.Fail("Code already used.");

            var token = GenerateSecureToken();

            context.DeviceToken.Add(
                new DeviceToken
                {
                    Token = token,
                    UserId = code.UserId,
                    DeviceName = request.DeviceName,
                    CreatedAt = _timeZoneService.NowJst,
                    IsRevoked = false
                }
            );

            code.IsUsed = true;

            await context.SaveChangesAsync();

            return ActivateResult.Success(token);
        }

        private string GenerateSecureToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }
    }
}
