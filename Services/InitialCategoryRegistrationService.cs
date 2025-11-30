using ActivityManagementApp.Models;
using ActivityManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class InitialCategoryRegistrationService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public InitialCategoryRegistrationService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateForUserAsync(string userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            if (await context.CategoryTypeMaster.AnyAsync(x => x.UserId == userId))
                return;

            int typeOrder = 1;
            int categoryOrder = 1;
            foreach (var group in DefaultCategorySeed.Groups)
            {
                var type = new CategoryTypeMaster
                {
                    TypeName = group.TypeName,
                    ColorKey = group.ColorKey,
                    TextColorKey = group.TextColorKey,
                    SortOrder = typeOrder++,
                    UserId = userId
                };

                context.CategoryTypeMaster.Add(type);

                foreach (var name in group.Categories)
                {
                    var cat = new CategoryMaster
                    {
                        CategoryName = name,
                        UserId = userId,
                        CategoryTypeMasterId = type.Id,
                        SortOrder = categoryOrder++
                    };
                    context.CategoryMaster.Add(cat);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}