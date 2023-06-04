using GroupProjectDepositCatalogWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GroupProjectDepositCatalogWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DepositModel> Deposits { get; set; }
        public DbSet<ApplicationUserModel> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUserModel>()
                 .HasMany(d => d.MyDeposits)
                 .WithOne(u => u.MyApplicationUser)
                 .HasForeignKey(d => d.MyApplicationUserId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SearchFormEntity>(sfe => {
                sfe.HasNoKey();
            });//every time you create a table you must remove the create  SearchFormEntity table because it is a useless table why why why
            //most of dotnet logic is baffling stack upon stack of contradicting statements......

            base.OnModelCreating(modelBuilder);//works only for Identity(account system that is maintained by dotnet) classes
        }

        public DbSet<GroupProjectDepositCatalogWebApp.Models.SearchFormEntity> SearchFormEntity { get; set; } = default!;
    }
}