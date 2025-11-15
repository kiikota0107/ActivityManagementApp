using Microsoft.EntityFrameworkCore;
using ActivityManagementApp.Data;
using ActivityManagementApp.Models;

namespace ActivityManagementApp.Services
{
    public class FindMasterServices
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public FindMasterServices(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<CategoryMaster>?> FindCategoryMaster()
        {
            var userId = await _userService.GetUserIdAsync();
            if (userId == null) return null;
            List<CategoryMaster> categoryMasters = await _context.CategoryMaster.Where(x => x.UserId == "" || x.UserId == userId).ToListAsync();
            return categoryMasters;
        }
        public async Task<List<CategoryTypeMaster>> FindCategoryTypeMaster()
        {
            string? userId = await _userService.GetUserIdAsync();

            return await _context.CategoryTypeMaster
                .Where(x => x.UserId == "" || x.UserId == userId)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }
}
