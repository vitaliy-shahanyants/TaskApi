using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using TodoApp.Configuration;
using TodoApp.Models.DTOs.Requests;
using TodoApp.Models.DTOs.Responses;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthController(UserManager<IdentityUser> userManager, 
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthenticationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use"
                        },
                        Success = false
                    });
                }

                var newUser = new IdentityUser(){Email = user.Email, UserName = user.Username};
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    return Ok(new AuthenticationResponse()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new AuthenticationResponse()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                        ,
                        Success = false
                    });
                }
            }

            return BadRequest(new AuthenticationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUSer = await _userManager.FindByEmailAsync(user.Email);
                

                if (existingUSer == null)
                {
                    return BadRequest(new AuthenticationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid Login Request"
                        },
                        Success = false
                    });
                }

                var isCorrectPass = await _userManager.CheckPasswordAsync(existingUSer, user.Password);

                if (!isCorrectPass)
                {
                    return BadRequest(new AuthenticationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid Login Request"
                        },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUSer);
                return Ok(new AuthenticationResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
                
            }
            return BadRequest(new AuthenticationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
            
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtToeknHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtToeknHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtToeknHandler.WriteToken(token);

            return jwtToken;
        }
    }
}