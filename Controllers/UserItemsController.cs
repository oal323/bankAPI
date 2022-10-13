using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bankAPI.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using bankAPI.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;    
using System.Text;  


namespace bankAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    
    public class UserItemsController : ControllerBase
    {
        private IConfiguration _config;    
    
        private readonly BankContext _context;

        public UserItemsController(BankContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/UserItems
        [Authorize]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            return await _context.User.ToListAsync();
        }

        // GET: api/UserItems/5
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet("{uname},{pword}")]
        public async Task<IActionResult> Login(string uname,string pword)
        {
              
            SHA256 mySHA256 = SHA256.Create();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(pword);
            data = mySHA256.ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            if(_context.User.Where(u => u.userName.Equals(uname)).Count()>=1&&_context.User.Where(u=>u.password.Equals(hash)).Count()>=1){
                User user = _context.User.Where(u => u.password.Equals(hash)&&u.userName.Equals(uname)).Include(acctDB=>acctDB.AcctsDB).First();
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
        (_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.userName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Ok(stringToken);
    }
        return Unauthorized();
        
          
            
        }
        // PUT: api/UserItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize]
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/UserItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.User == null)
          {
              return Problem("Entity set 'BankContext.User'  is null.");
          }
           string guid=System.Guid.NewGuid().ToString().Substring(21);
            user.id=guid;
            SHA256 mySHA256 = SHA256.Create();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(user.password);
            data = mySHA256.ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            user.password=hash;
            if(_context.User.Where(u => u.userName.Equals(user.userName)).Count()>=1){return Conflict();}
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }

        // DELETE: api/UserItems/5
        //[Authorize]
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_context.User?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
