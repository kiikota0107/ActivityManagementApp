using ActivityManagementApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityManagementApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ActivityLogs> ActivityLogs { get; set; }
    public DbSet<CategoryMaster> CategoryMaster { get; set; }
    public DbSet<CategoryTypeMaster> CategoryTypeMaster { get; set; }
    public DbSet<DevicePairingCode> DevicePairingCodes { get; set; }
    public DbSet<DevicePairingCode> DeviceTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ★ カテゴリ削除時に ActivityLogs.CategoryMasterId を NULL にする
        builder.Entity<ActivityLogs>()
            .HasOne(a => a.CategoryMaster)
            .WithMany()
            .HasForeignKey(a => a.CategoryMasterId)
            .OnDelete(DeleteBehavior.SetNull);

        // ★ ペアリングコードは一意にする
        builder.Entity<DevicePairingCode>()
            .HasIndex(x => x.Code)
            .IsUnique();
    }
}
