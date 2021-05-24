using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UntoldApp.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        //GET : /api/ApplicationUser
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<ApplicationUser>> GetCashiersAsync()
        {
            return await Task.Run(() =>
            {
                return _userManager.GetUsersInRoleAsync("Cashier").Result.ToList();
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("RegisterCashier")]
        //POST : /api/ApplicationUser/RegisterCashier
        public async Task<Object> PostCashier(ApplicationUserModel model)
        {
            model.Role = "Cashier";
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //PUT : /api/ApplicationUser
        [HttpPut("{param}")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> PutCashierModelWithPassword(string param, ApplicationUserModel model)
        {
            string id = param.Substring(0, param.Length - 1);
            bool withPassword = param.Substring(param.Length - 1, 1).Equals("1");
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            if (withPassword)
            {
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = newPasswordHash;
            }
            try
            {
                var result = await _userManager.UpdateAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //DELETE : /api/ApplicationUser
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicketModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            try
            {
                var result = await _userManager.DeleteAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            model.Role = "Admin";
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //Get role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }
    }
}