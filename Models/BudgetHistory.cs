
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cobbler.Models
{
    // A budget history is created each time a user creates an Allocation
    public class BudgetHistory
    {
        public int BudgetHistoryId { get; set; }
        public double Amount { get; set; }
        public string PersonName { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedDate { get; set; }

        // Association to Budget 
        public int BudgetPlanId { get; set; }
        public BudgetPlan BudgetPlan { get; set; }

        // Association to Allocations 
        public Allocation Allocation { get; set; }
    }

}