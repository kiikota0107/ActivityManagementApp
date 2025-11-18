using ActivityManagementApp.Models;
using ActivityManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Services
{
    public class InitialCategoryRegistrationService
    {
        private readonly ApplicationDbContext _db;

        public InitialCategoryRegistrationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateForUserAsync(string userId)
        {
            if (await _db.CategoryTypeMaster.AnyAsync(x => x.UserId == userId))
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

                _db.CategoryTypeMaster.Add(type);
                await _db.SaveChangesAsync();

                foreach (var name in group.Categories)
                {
                    var cat = new CategoryMaster
                    {
                        CategoryName = name,
                        UserId = userId,
                        CategoryTypeMasterId = type.Id,
                        SortOrder = categoryOrder++
                    };
                    _db.CategoryMaster.Add(cat);
                }

                await _db.SaveChangesAsync();
            }
        }
    }
}