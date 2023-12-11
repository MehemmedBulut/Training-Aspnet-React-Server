using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TrainPro.Data;
using TrainPro.Models;
using TrainPro.Models.Dto;
using TrainPro.Services;
using TrainPro.Utility;

namespace TrainPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(ApplicationDbContext db, IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            AppUser userFromDb = _db.AppUser
                .FirstOrDefault(U => U.UserName.ToLower() == model.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);


            if(isValid == false)
            {
                _response.Result = new LoginResponseDTO();
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSsuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            LoginResponseDTO loginResponse = new()
            {
                Email = userFromDb.Email,
                Token = "REPLACE WITH ACTUAL TOKEN ONCE WE GENERATE"
            };

            if(loginResponse.Email == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSsuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSsuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            AppUser userFromDb = _db.AppUser
                .FirstOrDefault(U=>U.UserName.ToLower() == model.UserName.ToLower());
            if(userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSsuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            AppUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Name = model.UserName,
            };
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }
                    if (model.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    }

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSsuccess = true;
                    return Ok(_response);
                }
            }
            catch(Exception)
            {

            }
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSsuccess = false;
            _response.ErrorMessages.Add("Error while registering");
            return Ok(_response);
        }

    }
}
