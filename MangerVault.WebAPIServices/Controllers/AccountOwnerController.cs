
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageUsers.Data;
using ManageUsers.Models;
using MongoDB.Driver;
using MangerVault.WebAPIServices.Models;
using System;

namespace ManageUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountOwnerController : ControllerBase
    {
        private readonly AccountOwnerContext _context;

        public AccountOwnerController(AccountOwnerContext context)
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
        public async Task<ActionResult<IEnumerable<AccountOwner>>> GetAccountOwnerItems()
        {
            return await _context.AccountOwnerItems.ToListAsync();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PostPasscodeAccount(Credential account)
        {
            // account.Id = Guid.NewGuid().ToString();

            //await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            var owners = await _context.AccountOwnerItems.ToListAsync();
            if (owners is not null)
            {
                foreach (var item in owners)
                {
                    if (!string.IsNullOrEmpty(account.password) && item.Credential != null && account.password != item.Credential.password)
                    {
                        return Unauthorized();
                    }
                }
            }
            return Ok();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountOwner>> PostAccount(AccountOwner account)
        {
           // account.Id = Guid.NewGuid().ToString();

            //await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            _context.AccountOwnerItems.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { Id = account.Id }, account);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountOwner>> GetAccount(string id)
        {
            var account = await _context.AccountOwnerItems.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, AccountOwner account)
        {
            //if (id != Convert.ToInt32(account.id))
            //{
            //    return BadRequest();
            //}
            account.Id = id;

            // Attach the entity to the context
            _context.AccountOwnerItems.Attach(account);

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
            var account = await _context.AccountOwnerItems.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.AccountOwnerItems.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string id)
        {
            return _context.AccountOwnerItems.Any(e => e.Id == Convert.ToString(id));
        }
    }
}
