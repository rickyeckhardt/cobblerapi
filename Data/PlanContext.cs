using Cobbler.Models;
using Microsoft.EntityFrameworkCore;

namespace Cobbler.Data

{
    public class PlanContext : DbContext
    {
        public PlanContext(DbContextOptions<PlanContext> options) : base(options)
        {
        }

        public DbSet<BudgetPlan> BudgetPlans { get; set; }
        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<BudgetHistory> BudgetHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BudgetPlan>().ToTable("BudgetPlan");
            modelBuilder.Entity<Allocation>().ToTable("Allocation");
            modelBuilder.Entity<BudgetHistory>().ToTable("BudgetHistory");

            modelBuilder.Entity<BudgetHistory>()
                .HasOne(b => b.Allocation)
                .WithOne(a => a.BudgetHistory);
        }
    }
}
