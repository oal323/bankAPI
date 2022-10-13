using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bankAPI.Models;

namespace bankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionItemsController : ControllerBase
    {
        private readonly BankContext _context;

        public TransactionItemsController(BankContext context)
        {
            _context = context;
        }

        // GET: api/TransactionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
          if (_context.Transaction == null)
          {
              return NotFound();
          }
            return await _context.Transaction.ToListAsync();
        }

        // GET: api/TransactionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(string id)
        {
          if (_context.Transaction == null)
          {
              return NotFound();
          }
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/TransactionItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(string id, Transaction transaction)
        {
            if (id != transaction.id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/TransactionItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
          if (_context.Transaction == null)
          {
              return Problem("Entity set 'BankContext.Transaction'  is null.");
          }
            _context.Transaction.Add(transaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTransaction", new { id = transaction.id }, transaction);
        }

        // DELETE: api/TransactionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            if (_context.Transaction == null)
            {
                return NotFound();
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(string id)
        {
            return (_context.Transaction?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
