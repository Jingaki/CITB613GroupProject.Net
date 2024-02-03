using GroupProjectDepositCatalogWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace GroupProjectDepositCatalogWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DepositModel> Deposits { get; set; }
        public DbSet<ShiftingInterestRateDataModel> ShiftingInterests_table { get; set; }
        public DbSet<ApplicationUserModel> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUserModel>()
                 .HasMany(d => d.MyDeposits)
                 .WithOne(u => u.MyApplicationUser)
                 .HasForeignKey(d => d.MyApplicationUserId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DepositModel>()
                .HasMany(s => s.ShiftingInteresData)
                .WithOne(d => d.Deposit)
                .HasForeignKey(s => s.DepositId)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);//works only for Identity(account system that is maintained by dotnet) classes

            modelBuilder.Ignore<SearchFormEntity>();
        }
    }
}