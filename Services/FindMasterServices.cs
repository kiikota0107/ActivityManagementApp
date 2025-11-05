using Microsoft.EntityFrameworkCore;
using ActivityManagementApp.Data;
using ActivityManagementApp.Models;

namespace ActivityManagementApp.Services
{
    public class FindMasterServices
    {
        private readonly ApplicationDbContext _context;

        public FindMasterServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryMaster>> FindCategoryMaster()
        {
            List<CategoryMaster> categoryMasters = await _context.CategoryMaster.ToListAsync();
            return categoryMasters;
        }
    }
}
