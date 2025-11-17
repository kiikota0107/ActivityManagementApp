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

            int maxOrder = await _context.CategoryTypeMaster
                .Where(x => x.UserId == userId)
                .Select(x => x.SortOrder)
                .DefaultIfEmpty(0)
                .MaxAsync();

            input.UserId = userId!;
            input.SortOrder = maxOrder + 1;

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
            bool hasChildren = await _context.CategoryMaster.AnyAsync(c => c.CategoryTypeMasterId == id);

            if (hasChildren) return false;

            var entity = await _context.CategoryTypeMaster.FindAsync(id);
            if (entity == null) return false;

            _context.CategoryTypeMaster.Remove(entity);
            await _context.SaveChangesAsync();

            var userId = await _userService.GetUserIdAsync();

            var list = await _context.CategoryTypeMaster
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            int order = 1;
            foreach (var record in list)
            {
                record.SortOrder = order++;
            }

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

            int maxOrder = await _context.CategoryMaster
                .Where(x => x.UserId == userId)
                .Select(x => x.SortOrder)
                .DefaultIfEmpty(0)
                .MaxAsync();

            input.SortOrder = maxOrder + 1;

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

            int typeId = entity.CategoryTypeMasterId;

            var logs = await _context.ActivityLogs
                            .Where(x => x.CategoryMasterId == id)
                            .ToListAsync();

            foreach (var log in logs)
            {
                log.CategoryMasterId = null;
            }

            _context.CategoryMaster.Remove(entity);
            await _context.SaveChangesAsync();

            var userId = await _userService.GetUserIdAsync();

            var list = await _context.CategoryMaster
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            int order = 1;
            foreach (var record in list)
            {
                record.SortOrder = order++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryTypeSortOrdersAsync(List<int> orderdIds)
        {
            var userId = await _userService.GetUserIdAsync();

            var list = await _context.CategoryTypeMaster
                                        .Where(x => x.UserId == userId)
                                        .ToListAsync();

            int order = 1;
            foreach (var id in orderdIds)
            {
                var item = list.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    item.SortOrder = order++;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategorySortOrdersAsync(List<int> orderdIds)
        {
            var userId = await _userService.GetUserIdAsync();

            var list = await _context.CategoryMaster
                                .Where(x => x.UserId == userId)
                                .ToListAsync();

            int order = 1;
            foreach (var id in orderdIds)
            {
                var item = list.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    item.SortOrder = order++;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
