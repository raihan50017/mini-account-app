using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniAccount.Models;
using mini_account_app.Models;

namespace mini_account_app.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MiniAccount.Models.AppIdentityRole> AppIdentityRole { get; set; } = default!;
        public DbSet<mini_account_app.Models.UserToModulePermission> UserToModulePermission { get; set; } = default!;
        public DbSet<mini_account_app.Models.ChartOfAccount> ChartOfAccounts { get; set; } = default!;
        public DbSet<mini_account_app.Models.VoucherEntry> VoucherEntry { get; set; } = default!;
        public DbSet<mini_account_app.Models.VoucherEntryDetails> VoucherEntryDetails { get; set; } = default!;
    }
}
