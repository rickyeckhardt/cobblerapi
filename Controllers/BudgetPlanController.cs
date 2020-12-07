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
    public class BudgetPlanController : ControllerBase
    {
        private readonly PlanContext _context;

        public BudgetPlanController(PlanContext context)
        {
            _context = context;
        }

        // GET: api/BudgetPlan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetPlan>>> GetBudgetPlans()
        {
            //return await _context.BudgetPlans.ToListAsync();

            var budgetPlans = await _context.BudgetPlans
                .Include(b => b.Allocations)
                .Include(b => b.BudgetHistories)
                .ToArrayAsync();
 
            var response = budgetPlans.Select(b => new
            {
                budget_id = b.Id,
                plan_name = b.PlanName,
                budget_amount = b.BudgetAmount,
                allocations = b.Allocations.Select(a => new {
                    allocation_id = a.AllocationId,
                    person_name = a.PersonName,
                    project_name = a.ProjectName,
                    budget_amount = a.AllocatedAmount
                }),
                budget_history = b.BudgetHistories.Select(b => new {
                    history_id = b.BudgetHistoryId,
                    history = "$" + b.Amount + " was allocated to " + b.PersonName + " for " + b.ProjectName + "."
                }),
            });
 
            return Ok(response);
        }

        // GET: api/BudgetPlan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetPlan>> GetBudgetPlan(int id)
        {
            var budgetPlan = await _context.BudgetPlans.FindAsync(id);

            if (budgetPlan == null)
            {
                return NotFound();
            }

            return budgetPlan;
        }

        // PUT: api/BudgetPlan/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudgetPlan(int id, BudgetPlan budgetPlan)
        {
            if (id != budgetPlan.Id)
            {
                return BadRequest();
            }

            _context.Entry(budgetPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetPlanExists(id))
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

        // POST: api/BudgetPlan
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetPlan>> PostBudgetPlan(BudgetPlan budgetPlan)
        {
            _context.BudgetPlans.Add(budgetPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetPlan", new { id = budgetPlan.Id }, budgetPlan);
        }

        // DELETE: api/BudgetPlan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetPlan(int id)
        {
            var budgetPlan = await _context.BudgetPlans.FindAsync(id);
            if (budgetPlan == null)
            {
                return NotFound();
            }

            _context.BudgetPlans.Remove(budgetPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetPlanExists(int id)
        {
            return _context.BudgetPlans.Any(e => e.Id == id);
        }
    }
}
