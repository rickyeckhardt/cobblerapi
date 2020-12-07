using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cobbler.Data;
using Cobbler.Models;

namespace Cobbler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private readonly PlanContext _context;

        public AllocationController(PlanContext context)
        {
            _context = context;
        }

        // GET: api/Allocation defaults to get results by name
        // api/Allocation?orderBy=Person returns by person name
        // api/Allocation?orderBy=Project returns by project name
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Allocation>>> GetAllocations(string orderBy)
        {

            var allocations = await _context.Allocations.ToListAsync();

            var response = allocations.GroupBy(a => a.PersonName).Select(a => a).ToList();

            switch (orderBy)
            {
                case "Person":
                    response = allocations.GroupBy(a => a.PersonName).Select(a => a).ToList();
                    break;
                case "Project":
                    response = allocations.GroupBy(a => a.ProjectName).Select(a => a).ToList();
                    break;
                default:
                    response = allocations.GroupBy(a => a.PersonName).Select(a => a).ToList();
                    break;
            }
            
            return Ok(response);
        }

        // GET: api/Allocation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Allocation>> GetAllocation(int id)
        {
            var allocation = await _context.Allocations.FindAsync(id);

            if (allocation == null)
            {
                return NotFound();
            }

            return allocation;
        }

        // PUT: api/Allocation/5
        // {
        //      "AllocationId": 1,
        //      "PersonName": "Jane",
        //      "ProjectName": "SEO",
        //      "AllocatedAmount": 500.00,
        //      "BudgetPlanId": 1,
        //      "BudgetHistoryId": 1               - This history would change each time an allocation is updated. i.e. 5 would be next 
        // }
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllocation(int id, Allocation allocation)
        {
            if (id != allocation.AllocationId)
            {
                return BadRequest();
            }

            // Get the current budget amount
            var budgetPlan = _context.BudgetPlans.Single(b => b.Id == allocation.BudgetPlanId);

            // Get the current allocated amount
            var currentAllocation = await _context.BudgetHistories.FindAsync(allocation.BudgetHistoryId);

            // Update the budget amount
            if ( budgetPlan.BudgetAmount > 0 && (budgetPlan.BudgetAmount - allocation.AllocatedAmount > 0)) {
                var allocationDifference = currentAllocation.Amount - allocation.AllocatedAmount;

                var updatedBudgetAmount = 0.0;

                updatedBudgetAmount = budgetPlan.BudgetAmount + allocationDifference;
                
                budgetPlan.BudgetAmount = updatedBudgetAmount;
                
                await _context.SaveChangesAsync();
            } else {

                return BadRequest();
            };

            // Create the history of the allocation 
            var budgetHistory = new BudgetHistory {
                Amount = allocation.AllocatedAmount,
                PersonName = allocation.PersonName,
                ProjectName = allocation.ProjectName,
                CreatedDate = DateTime.Now,
                BudgetPlanId = allocation.BudgetPlanId
            };

            _context.BudgetHistories.Add(budgetHistory);
            await _context.SaveChangesAsync();

            allocation.BudgetHistoryId = budgetHistory.BudgetHistoryId;

            _context.Entry(allocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Allocation
        // {
        //      "PersonName": "Jane",
        //      "ProjectName": "SEO",
        //      "AllocatedAmount": 500.00,
        //      "BudgetPlanId": 1
        // }
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Allocation>> PostAllocation(Allocation allocation)
        {   
            // Get the current budget amount
            var budgetPlan = _context.BudgetPlans.Single(b => b.Id == allocation.BudgetPlanId);

            // Update the budget amount
            if ( budgetPlan.BudgetAmount > 0 && (budgetPlan.BudgetAmount - allocation.AllocatedAmount > 0)) {
                var updatedBudgetAmount = budgetPlan.BudgetAmount - allocation.AllocatedAmount;

                budgetPlan.BudgetAmount = updatedBudgetAmount;
                
                await _context.SaveChangesAsync();
            } else {
                return BadRequest();
            };

            // Create the history of the allocation 
            var budgetHistory = new BudgetHistory {
                Amount = allocation.AllocatedAmount,
                PersonName = allocation.PersonName,
                ProjectName = allocation.ProjectName,
                CreatedDate = DateTime.Now,
                BudgetPlanId = allocation.BudgetPlanId
            };

            _context.BudgetHistories.Add(budgetHistory);
            await _context.SaveChangesAsync();

            // Create the allocation
            var newAllocation = new Allocation {
                PersonName = allocation.PersonName,
                ProjectName = allocation.ProjectName,
                BudgetPlanId = allocation.BudgetPlanId,
                AllocatedAmount = allocation.AllocatedAmount,
                BudgetHistoryId = budgetHistory.BudgetHistoryId
            };

            _context.Allocations.Add(newAllocation);
            await _context.SaveChangesAsync();

            return Ok(newAllocation);
        }

        // DELETE: api/Allocation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllocation(int id)
        {
            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation == null)
            {
                return NotFound();
            }

            _context.Allocations.Remove(allocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AllocationExists(int id)
        {
            return _context.Allocations.Any(e => e.AllocationId == id);
        }
    }
}
