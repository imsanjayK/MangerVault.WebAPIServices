
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageUsers.Models;
using MongoDB.Driver;
using MangerVault.WebAPIServices.Models;
using MangerVault.WebAPIServices.UtilityHelper;

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

        // GET: api/Accounts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AccountOwner>>> GetAccountOwnerItems()
        //{
        //    return await _context.AccountOwnerItems.ToListAsync();
        //}

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PasscodeLoginAccount(Credential loginUser)
        {
            if (loginUser is null)
            {
                return BadRequest();
            }

            await _context.Database.EnsureCreatedAsync();
            var dbUser = await _context.AccountOwnerItems.Where(o => o.Credential.Username == loginUser.Username).FirstOrDefaultAsync();
            
            if (dbUser is null)
            {
                return Unauthorized();
            }

            var isAuthorized = PasswordHelper.VerifyPassword(loginUser?.Password, dbUser.Credential);
           
            return isAuthorized? Ok() : Unauthorized();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountOwner>> PostAccount(AccountOwner account)
        {
            await _context.Database.EnsureCreatedAsync();

            if (account is null)
            {
                return BadRequest();
            }

            if (account?.Credential?.Password is not null)
            {
                var store = PasswordHelper.HashPassword(account.Credential.Password);

                account.Credential.Password = store.hashedPassword;
                account.Credential.Passphrase = store.salt;
                _context.AccountOwnerItems.Add(account);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction(nameof(GetAccount), new { account.Id }, 
            new AccountOwner
            {
                Id = account.Id,
                Contact = account.Contact,
                Credential = new Credential
                {
                    Username = account?.Credential?.Username
                }
            });
        }
        // GET: api/Accounts/sanjay
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountOwner>> GetAccount(string id)
        {
            var account = await _context.AccountOwnerItems.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return new AccountOwner
            {
                Id = account.Id,
                Contact = account.Contact,
                Credential = new Credential
                {
                    Username = account?.Credential?.Username
                }
            };
        }

        // GET: api/Accounts/sanjay
        [HttpGet("{username}")]
        public async Task<ActionResult<AccountOwner>> GetAccountByName(string username)
        {
            var account = await _context.AccountOwnerItems.Where(o => o.Credential.Username == username).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAccount(string id, AccountOwner account)
        //{
        //    if (id != account.Id)
        //    {
        //        return BadRequest();
        //    }
        //    account.Id = id;

        //    // Attach the entity to the context
        //    _context.AccountOwnerItems.Attach(account);

        //    _context.Entry(account).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AccountExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        // DELETE: api/Accounts/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAccount(string id)
        //{
        //    var account = await _context.AccountOwnerItems.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.AccountOwnerItems.Remove(account);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool AccountExists(string id)
        //{
        //    return _context.AccountOwnerItems.Any(e => e.Id == Convert.ToString(id));
        //}
    }
}
