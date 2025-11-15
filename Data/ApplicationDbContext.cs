using ActivityManagementApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ActivityLogs> ActivityLogs { get; set; }
    public DbSet<CategoryMaster> CategoryMaster { get; set; }
    public DbSet<CategoryTypeMaster> CategoryTypeMaster { get; set; }
}
