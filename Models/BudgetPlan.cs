
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cobbler.Models
{
    // Budget Plan holds the allocations and histories for a user
    public class BudgetPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        // Initital budget amount set in DbInitializer, but accounts for other data initialized i.e. $10000 minus the money allocated already
        public double BudgetAmount { get; set; }
        public ICollection<Allocation> Allocations { get; set; }

        public ICollection<BudgetHistory> BudgetHistories { get; set; }
    }

}