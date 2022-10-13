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
    
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountItemsController : ControllerBase
    {
        private readonly BankContext _context;

        public AccountItemsController(BankContext context)
        {
            _context = context;
        }

        // GET: api/AccountItems
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
          if (_context.Account == null)
          {
              return NotFound();
          }
            return await _context.Account.ToListAsync();
        }

        // GET: api/AccountItems/5
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
          if (_context.Account == null)
          {
              return NotFound();
          }
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
        // PUT: api/AccountItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            if (id != account.id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/AccountItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
          if (_context.Account == null)
          {
              return Problem("Entity set 'BankContext.Account'  is null.");
          }
            User user = _context.User.Where(u => u.id.Equals(account.userid)).Include(acctDB=>acctDB.AcctsDB).ToList().First();
            account.User=user;
            account.id=System.Guid.NewGuid().ToString().Substring(21);
            _context.Account.Add(account);
            Console.WriteLine(_context.ChangeTracker.DebugView.LongView);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(account.id);
        }

        // DELETE: api/AccountItems/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            if (_context.Account == null)
            {
                return NotFound();
            }
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string id)
        {
            return (_context.Account?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
