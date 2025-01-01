
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageUsers.Data;
using ManageUsers.Models;
using MongoDB.Driver;

namespace ManageUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountContext _context;

        public AccountController(AccountContext context)
        {
            _context = context;
           
        }
        //// GET: api/accounts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        //{
        //    var accounts = await _context.Accounts.Find(account => true).ToListAsync();
        //    return Ok(accounts);
        //}

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountItems()
        {
            return await _context.AccountItems.ToListAsync();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
           // account.Id = Guid.NewGuid().ToString();

            //await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            _context.AccountItems.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.id }, account);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _context.AccountItems.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            //if (id != Convert.ToInt32(account.id))
            //{
            //    return BadRequest();
            //}
            account.id = id;

            // Attach the entity to the context
            _context.AccountItems.Attach(account);

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


        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var account = await _context.AccountItems.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.AccountItems.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string id)
        {
            return _context.AccountItems.Any(e => e.id == Convert.ToString(id));
        }
    }
}
