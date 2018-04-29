using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HiHostelFes.API.Data;
using HiHostelFes.API.Dtos;
using HiHostelFes.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace HiHostelFes.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthAdminController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthAdminController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }
        //HttpPost get informations from the users
        [HttpPost("register")]
        //FromBody to check informtion from the body 
        public async Task<IActionResult> Register([FromBody]AdminForLoginDto adminForLoginDto)
        {
            if(!string.IsNullOrEmpty(adminForLoginDto.Username))
            adminForLoginDto.Username = adminForLoginDto.Username.ToLower();                
            if (await _repo.UserExists(adminForLoginDto.Username))
                ModelState.AddModelError("Username", "Username Already exists");
            // validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userToCreate = new Admin
            {
                Username = adminForLoginDto.Username
            };
            var createdUser = await _repo.Register(userToCreate, adminForLoginDto.Password);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]AdminForLoginDto adminForLoginDto)
        {
           
                var userFromRepo = await _repo.Login(
                adminForLoginDto.Username.ToLower(),
                adminForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();
            //generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key =Encoding.ASCII.GetBytes(_config.GetSection("AppSetting:Token").Value);
            var tokenDecriptor = new SecurityTokenDescriptor
            {
                //This part is for payload of our token
                Subject = new ClaimsIdentity
                (
                    new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                            new Claim(ClaimTypes.Name,userFromRepo.Username)
                        }
                        
                ),
                //Expires after 24 hours
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new
                SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)

            };
            var token = tokenHandler.CreateToken(tokenDecriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { tokenString });           
        }
    }
}