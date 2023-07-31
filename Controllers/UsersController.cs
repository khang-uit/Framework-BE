using Memoriesx.Models;
using Memoriesx.Models.Dto;
using Memoriesx.Service.Posts;
using Memoriesx.Service.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memoriesx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public string secret = "secret";
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;
        }
  
        // Post: api/<UsersController/>
        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            //CreatePasswordHash(user.Password, out byte[] passwordHash);
            
            //user.Password = passwordHash;

            var userGot = _usersService.SignUp(user);
            string token = CreateToken(userGot);
            return Ok(new { result = userGot, token });
        }
        // Post: api/<UsersController/>
        [HttpPost("SignIn")]
        public IActionResult SignIn([FromBody] LoginDto login)
        {
            if (login == null)
            {
                return BadRequest();
            }
            var userGot = _usersService.SignIn(login.Email, login.Password);
 //           if (!VerifyPasswordHash(login.Password, userGot.Password))
            if (userGot == null)
            {
                return NotFound(new { message = "User does not exist or wrong password" });
            }
            string token = CreateToken(userGot);
            return Ok(new { result = userGot, token });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value
                ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
