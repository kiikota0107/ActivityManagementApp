using Microsoft.EntityFrameworkCore;
using ActivityManagementApp.Data;
using ActivityManagementApp.Models;

namespace ActivityManagementApp.Services
{
    public class FindMasterServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public FindMasterServices(IDbContextFactory<ApplicationDbContext> contextFactory, UserService userService, IConfiguration config)
        {
            _contextFactory = contextFactory;
            _userService = userService;
            _config = config;
        }

        public async Task<List<CategoryMaster>?> FindCategoryMaster()
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var userId = await _userService.GetUserIdAsync();
            if (userId == null) return null;
            List<CategoryMaster> categoryMasters = await context.CategoryMaster
                                                                    .Include(x => x.CategoryTypeMaster)
                                                                    .Where(x => x.UserId == userId)
                                                                    .OrderBy(x => x.Id)
                                                                    .ToListAsync();

            var guestUserId = await _userService.GetUserIdByEmailAsync(_config["GuestUser:Email"]);

            if (userId == guestUserId)
            {
                var demoUserId = await _userService.GetUserIdByEmailAsync(_config["DemoOwner:Email"]);
                List<CategoryMaster> demoCategoryMasters = await context.CategoryMaster
                                                                            .Include(x => x.CategoryTypeMaster)
                                                                            .Where(x => x.UserId == demoUserId)
                                                                            .OrderBy (x => x.Id)
                                                                            .ToListAsync();
                categoryMasters.AddRange(demoCategoryMasters);
            }

            return categoryMasters.OrderBy(x => x.Id).ToList();
        }

        public async Task<List<CategoryTypeMaster>> FindCategoryTypeMaster()
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            string? userId = await _userService.GetUserIdAsync();

            List<CategoryTypeMaster> categoryTypeMasters = await context.CategoryTypeMaster
                                                                            .Where(x => x.UserId == "" || x.UserId == userId)
                                                                            .OrderBy(x => x.Id)
                                                                            .ToListAsync();

            var guestUserId = await _userService.GetUserIdByEmailAsync(_config["GuestUser:Email"]);

            if (userId == guestUserId)
            {
                var demoUserId = await _userService.GetUserIdByEmailAsync(_config["DemoOwner:Email"]);

                List<CategoryTypeMaster> demoCategoryTypeMasters = await context.CategoryTypeMaster
                                                                                .Where(x => x.UserId == "" || x.UserId == demoUserId)
                                                                                .OrderBy(x => x.Id)
                                                                                .ToListAsync();

                categoryTypeMasters.AddRange(demoCategoryTypeMasters);
            }

            return categoryTypeMasters.OrderBy(x => x.Id).ToList();
        }
    }
}
