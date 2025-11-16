using ActivityManagementApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ActivityLogs> ActivityLogs { get; set; }
    public DbSet<CategoryMaster> CategoryMaster { get; set; }
    public DbSet<CategoryTypeMaster> CategoryTypeMaster { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // š ƒJƒeƒSƒŠíœ‚É ActivityLogs.CategoryMasterId ‚ğ NULL ‚É‚·‚é
        builder.Entity<ActivityLogs>()
            .HasOne(a => a.CategoryMaster)
            .WithMany()
            .HasForeignKey(a => a.CategoryMasterId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
