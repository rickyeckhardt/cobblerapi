
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobbler.Models;

namespace Cobbler.Data

{
    // Inits a plan and a few allocations
    public class DbInitializer
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PlanContext(
                serviceProvider.GetRequiredService<DbContextOptions<PlanContext>>()))
            {
                if (context.BudgetPlans.Any())
                {
                    return;   // Database has been seeded
                }

                context.BudgetPlans.AddRange(
                    new BudgetPlan
                    {
                        Id = 1,
                        PlanName = "Cobbler",
                        BudgetAmount = 5300.00
                    });

                context.SaveChanges();

                context.Allocations.AddRange(
                    new Allocation
                    {
                        AllocationId = 1,
                        PersonName = "Jane",
                        ProjectName = "SEO",
                        BudgetPlanId = 1,
                        AllocatedAmount = 1000.00,
                        BudgetHistoryId = 1
                    },
                    new Allocation
                    {
                        AllocationId = 2,
                        PersonName = "Jane",
                        ProjectName = "Any Project",
                        BudgetPlanId = 1,
                        AllocatedAmount = 1200.00,
                        BudgetHistoryId = 2
                    },
                    new Allocation
                    {
                        AllocationId = 3,
                        PersonName = "Chris",
                        ProjectName = "SEO",
                        BudgetPlanId = 1,
                        AllocatedAmount = 500.00,
                        BudgetHistoryId = 3
                    },
                    new Allocation
                    {
                        AllocationId = 4,
                        PersonName = "Any Person",
                        ProjectName = "SEO",
                        BudgetPlanId = 1,
                        AllocatedAmount = 2000.00,
                        BudgetHistoryId = 4
                    });

                context.SaveChanges();

                context.BudgetHistories.AddRange(
                    new BudgetHistory
                    {
                        BudgetHistoryId = 1,
                        Amount = 1000.00,
                        PersonName = "Jane",
                        ProjectName = "SEO",
                        CreatedDate = DateTime.Now,
                        BudgetPlanId = 1
                    },
                    new BudgetHistory
                    {
                        BudgetHistoryId = 2,
                        Amount = 1200.00,
                        PersonName = "Jane",
                        ProjectName = "Any Project",
                        CreatedDate = DateTime.Now,
                        BudgetPlanId = 1
                    },
                    new BudgetHistory
                    {
                        BudgetHistoryId = 3,
                        Amount = 500.00,
                        PersonName = "Chris",
                        ProjectName = "SEO",
                        CreatedDate = DateTime.Now,
                        BudgetPlanId = 1
                    },
                    new BudgetHistory
                    {
                        BudgetHistoryId = 4,
                        Amount = 2000.00,
                        PersonName = "Any Person",
                        ProjectName = "SEO",
                        CreatedDate = DateTime.Now,
                        BudgetPlanId = 1
                    });

                context.SaveChanges();
            }
        }
    }
}