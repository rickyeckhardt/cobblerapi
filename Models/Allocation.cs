
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cobbler.Models
{
    // Individual Allocations made against the BudgetPlan
    public class Allocation
    {
        public int AllocationId { get; set; }
        public string PersonName { get; set; }
        public string ProjectName { get; set; }
        public double AllocatedAmount { get; set; }

        // Assocation to Budget Plan
        public int BudgetPlanId { get; set; }
        public BudgetPlan BudgetPlan { get; set; }

        // Assocation with a Budget History
        public int BudgetHistoryId { get; set; }
        public BudgetHistory BudgetHistory { get; set; }
    }

}