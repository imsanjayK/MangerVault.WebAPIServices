
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageUsers.Data;
using ManageUsers.Models;
using MongoDB.Driver;
using MangerVault.WebAPIServices.UtilityHelper;

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

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountItems()
        {
            return await _context.AccountItems.ToListAsync();
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            await _context.Database.EnsureCreatedAsync();

            var credential = new List<Credential>();
            foreach (var cred in account.credentials)
            {
                var passphrase = PasswordHelper.GenerateRandomString();
                credential.Add(new Credential
                {
                    Username = cred.Username,
                    Password = PasswordHelper.EncryptPassword(cred.Password, passphrase),
                    Passphrase = passphrase
                });
            }

            account.credentials = credential;
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

            var credential = new List<Credential>();
            foreach (var cred in account.credentials)
            {
                credential.Add(new Credential
                {
                    Username = cred.Username,
                    Password = PasswordHelper.DecryptPassword(cred.Password, cred.Passphrase)
                });
            }

            account.credentials = credential;
            return account;
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            account.id = id;

            var credential = new List<Credential>();
            foreach (var cred in account.credentials)
            {
                var passphrase = PasswordHelper.GenerateRandomString();
                credential.Add(new Credential
                {
                    Username = cred.Username,
                    Password = PasswordHelper.EncryptPassword(cred.Password, passphrase),
                    Passphrase = passphrase
                });
            }

            account.credentials = credential;

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
