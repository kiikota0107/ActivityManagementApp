using ActivityManagementApp.Data;
using ActivityManagementApp.Models;
using ActivityManagementApp.Domain.Validators;
using Microsoft.EntityFrameworkCore;


namespace ActivityManagementApp.Services
{
    public class CategorySettingsService
    {
        IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserService _userService;

        public CategorySettingsService(IDbContextFactory<ApplicationDbContext> contextFactory, UserService userService)
        {
            _contextFactory = contextFactory;
            _userService = userService;
        }

        // ============================================
        //  CategoryTypeMaster CRUD
        // ============================================

        public async Task InsertCategoryTypeAsync(CategoryTypeMaster input)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var userId = await _userService.GetUserIdAsync();

            int maxOrder = await context.CategoryTypeMaster
                .Where(x => x.UserId == userId)
                .MaxAsync(x => (int?)x.SortOrder) ?? 0;

            input.UserId = userId!;
            input.SortOrder = maxOrder + 1;

            await context.CategoryTypeMaster.AddAsync(input);
            await context.SaveChangesAsync();
        }

        public async Task<CustomValidationResult> UpdateCategoryTypeAsync(CategoryTypeMaster input)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.CategoryTypeMaster
                                        .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null) return CustomValidationResult.InValid("更新対象のレコードが見つかりませんでした。");

            var userId = await _userService.GetUserIdAsync();
            var result = CommonValidator.ValidateUserIsOwner(entity.UserId, userId ?? "");

            if (!result.IsValid) return CustomValidationResult.InValid(result.ErrorMessage);

            entity.TypeName = input.TypeName;
            entity.ColorKey = input.ColorKey;
            entity.TextColorKey = input.TextColorKey;
            entity.SortOrder = input.SortOrder;

            await context.SaveChangesAsync();
            return CustomValidationResult.Valid();
        }

        public async Task<CustomValidationResult> DeleteCategoryTypeAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.CategoryTypeMaster.FindAsync(id);
            if (entity == null) return CustomValidationResult.InValid("更新対象のレコードが見つかりませんでした。");

            var userId = await _userService.GetUserIdAsync();
            var result = CommonValidator.ValidateUserIsOwner(entity.UserId, userId ?? "");
            if (!result.IsValid) return result;

            bool hasChildren = await context.CategoryMaster.AnyAsync(c => c.CategoryTypeMasterId == id);
            if (hasChildren) return CustomValidationResult.InValid("このカテゴリタイプは使用されているため削除できません。");

            context.CategoryTypeMaster.Remove(entity);
            await context.SaveChangesAsync();

            var list = await context.CategoryTypeMaster
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            int order = 1;
            foreach (var record in list)
            {
                record.SortOrder = order++;
            }

            await context.SaveChangesAsync();

            return CustomValidationResult.Valid();
        }

        public async Task<CustomValidationResult> UpdateCategoryTypeSortOrdersAsync(List<CategoryTypeMaster> input)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            if (input == null) return CustomValidationResult.InValid("更新対象のレコードがありません。");

            var userId = await _userService.GetUserIdAsync();

            var result = CommonValidator.ValidateMixUser(input, userId ?? "");
            if (!result.IsValid) return result;

            var list = await context.CategoryTypeMaster
                                        .Where(x => x.UserId == userId)
                                        .ToListAsync();

            int order = 1;
            var orderedIds = input.Select(x => x.Id).ToList();

            foreach (var id in orderedIds)
            {
                var item = list.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    item.SortOrder = order++;
                }
            }

            await context.SaveChangesAsync();
            return CustomValidationResult.Valid();
        }

        // ============================================
        //  CategoryMaster CRUD
        // ============================================

        public async Task InsertCategoryAsync(CategoryMaster input)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var userId = await _userService.GetUserIdAsync();
            input.UserId = userId!;

            int maxOrder = await context.CategoryMaster
                .Where(x => x.UserId == userId)
                .MaxAsync(x => (int?)x.SortOrder) ?? 0;

            input.SortOrder = maxOrder + 1;

            await context.CategoryMaster.AddAsync(input);
            await context.SaveChangesAsync();
        }

        public async Task<CustomValidationResult> UpdateCategoryAsync(CategoryMaster input)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.CategoryMaster
                                        .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null) return CustomValidationResult.InValid("更新対象のレコードが見つかりませんでした。");

            var userId = await _userService.GetUserIdAsync();
            var result = CommonValidator.ValidateUserIsOwner(entity.UserId, userId ?? "");

            if (!result.IsValid) return result;

            entity.CategoryName = input.CategoryName;
            entity.CategoryTypeMasterId = input.CategoryTypeMasterId;
            entity.SortOrder = input.SortOrder;

            await context.SaveChangesAsync();
            return CustomValidationResult.Valid();
        }

        public async Task<CustomValidationResult> DeleteCategoryAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.CategoryMaster.FindAsync(id);
            if (entity == null) return CustomValidationResult.InValid("更新対象のレコードが見つかりませんでした。");

            var userId = await _userService.GetUserIdAsync();
            var result = CommonValidator.ValidateUserIsOwner(entity.UserId, userId ?? "");
            if (!result.IsValid) return result;

            int typeId = entity.CategoryTypeMasterId;

            var logs = await context.ActivityLogs
                            .Where(x => x.CategoryMasterId == id)
                            .ToListAsync();

            foreach (var log in logs)
            {
                log.CategoryMasterId = null;
            }

            context.CategoryMaster.Remove(entity);
            await context.SaveChangesAsync();

            var list = await context.CategoryMaster
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            int order = 1;
            foreach (var record in list)
            {
                record.SortOrder = order++;
            }

            await context.SaveChangesAsync();
            return CustomValidationResult.Valid();
        }

        public async Task<CustomValidationResult> UpdateCategorySortOrdersAsync(List<CategoryMaster> input)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            if (input == null) return CustomValidationResult.InValid("更新対象のレコードがありません。");

            var userId = await _userService.GetUserIdAsync();

            var result = CommonValidator.ValidateMixUser(input, userId ?? "");
            if (!result.IsValid) return result;

            var list = await context.CategoryMaster
                                .Where(x => x.UserId == userId)
                                .ToListAsync();

            int order = 1;
            var orderedIds = input.Select(x => x.Id).ToList();
            foreach (var id in orderedIds)
            {
                var item = list.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    item.SortOrder = order++;
                }
            }

            await context.SaveChangesAsync();
            return CustomValidationResult.Valid();
        }
    }
}