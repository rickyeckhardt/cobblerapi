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
    public class BudgetHistoryController : ControllerBase
    {
        private readonly PlanContext _context;

        public BudgetHistoryController(PlanContext context)
        {
            _context = context;
        }

        // GET: api/BudgetHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetHistory>>> GetBudgetHistories()
        {
            return await _context.BudgetHistories.ToListAsync();
        }

        // POST: api/BudgetHistory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetHistory>> PostBudgetHistory(BudgetHistory budgetHistory)
        {
            _context.BudgetHistories.Add(budgetHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetHistory", new { id = budgetHistory.BudgetHistoryId }, budgetHistory);
        }

        private bool BudgetHistoryExists(int id)
        {
            return _context.BudgetHistories.Any(e => e.BudgetHistoryId == id);
        }
    }
}
