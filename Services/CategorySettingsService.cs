using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using Microsoft.EntityFrameworkCore;


namespace ActivityManagementApp.Services
{
    public class CategorySettingsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public CategorySettingsService(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // ============================================
        //  CategoryTypeMaster CRUD
        // ============================================

        public async Task InsertCategoryTypeAsync(CategoryTypeMaster input)
        {
            var userId = await _userService.GetUserIdAsync();
            input.UserId = userId!;

            await _context.CategoryTypeMaster.AddAsync(input);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryTypeAsync(CategoryTypeMaster input)
        {
            var entity = await _context.CategoryTypeMaster
                                        .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null) return;

            entity.TypeName = input.TypeName;
            entity.ColorKey = input.ColorKey;
            entity.TextColorKey = input.TextColorKey;
            entity.SortOrder = input.SortOrder;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryTypeAsync(int id)
        {
            // 子カテゴリの存在チェック
            bool hasChildren = await _context.CategoryMaster.AnyAsync(c => c.CategoryTypeMasterId == id);

            if (hasChildren) return false; // 呼び出しもとで「削除不可メッセージ」を出す

            var entity = await _context.CategoryTypeMaster.FindAsync(id);
            if (entity == null) return false;

            _context.CategoryTypeMaster.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // ============================================
        //  CategoryMaster CRUD
        // ============================================
        
        public async Task InsertCategoryAsync(CategoryMaster input)
        {
            var userId = await _userService.GetUserIdAsync();
            input.UserId = userId!;

            await _context.CategoryMaster.AddAsync(input);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryMaster input)
        {
            var entity = await _context.CategoryMaster
                                        .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null) return;

            entity.CategoryName = input.CategoryName;
            entity.CategoryTypeMasterId = input.CategoryTypeMasterId;
            entity.SortOrder = input.SortOrder;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var entity = await _context.CategoryMaster.FindAsync(id);
            if (entity == null) return;

            // ★ ActivityLogs → CategoryMasterId を null にする
            var logs = await _context.ActivityLogs
                            .Where(x => x.CategoryMasterId == id)
                            .ToListAsync();

            foreach (var log in logs)
            {
                log.CategoryMasterId = null;
            }
        }
    }
}
