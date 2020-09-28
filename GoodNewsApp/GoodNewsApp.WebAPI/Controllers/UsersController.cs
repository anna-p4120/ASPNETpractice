using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Repository;
using GoodNewsApp.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoodNewsApp.BusinessLogic.Services.UsersServices;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using GoodNewsApp.BusinessLogic.Helpers;
using Microsoft.Extensions.Options;
using System.Threading;
using GoodNewsApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GoodNewsApp.BusinessLogic.Interfaces;

namespace GoodNewsApp.WebAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        
        
        private readonly AppSettings _appSettings;
        private readonly IUsersService _usersService;

        public static CancellationToken cancellationToken = new CancellationTokenSource().Token;

        public UsersController(IUsersService usersService, IOptions<AppSettings> appSettings)
        {          
            
            _usersService = usersService;

            _appSettings = appSettings.Value;


        }


       
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel registerModel) //
        {
            if (ModelState.IsValid) 
            {

                UserDTO userFromDB = await _usersService.GetUserByEmailAsync(registerModel.Email);

                if (userFromDB == null)
                {
                    try
                    {
                        string passwordHash, passwordSalt;

                        PasswordManager.CreatePasswordHash(registerModel.Password, out passwordHash, out passwordSalt);

                        UserDTO newUserDTO = new UserDTO()
                        {
                            Id = Guid.NewGuid(),
                            Name = registerModel.Name ?? registerModel.Email,
                            Email = registerModel.Email,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt
                        };

                        CreatedUserRole newUserParams = await _usersService.CreateUserRoleAsync(newUserDTO, DefaultRolesList.User.ToString()); //, RolesList.Admin.ToString()"Admin", 

                        return Ok(newUserDTO);


                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }

                }
                return BadRequest(new { message = "Такой пользователь существует" });

            }
            return BadRequest(new { message = "Username or password is incorrect" });


        }


       [AllowAnonymous]
       [HttpPost]
       [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel) 
        {
            //!string.IsNullOrEmpty(loginModel.Email) && !string.IsNullOrEmpty(loginModel.Password)
            
            if (ModelState.IsValid)
            {
                UserDTO userFromDB = await _usersService.GetUserByEmailAsync(loginModel.Email);

                if (userFromDB != null)
                {

                    if (PasswordManager.VerifyPasswordHash(loginModel.Password, userFromDB.PasswordHash, userFromDB.PasswordSalt))
                    {
                        List<Guid> userFromDBRoleId = _usersService.GetRoleIdByUserId(userFromDB.Id);
                        Guid roleID = userFromDBRoleId[0];

                        string tokenString = PasswordManager.CreateToken(userFromDB.Name, roleID, _appSettings.Secret);


                        // return basic user info and authentication token
                       // return Ok(tokenString);

                        return Ok(new
                        {
                            Id = userFromDB.Id,
                            Name = userFromDB.Name,
                            Email = userFromDB.Email,
                            UserRole = roleID,
                            Token = tokenString
                        });

                    }

                }
                return BadRequest(new { message = "User don't exist"});

            }
            return BadRequest(new { message = "Username or password is incorrect" });
 
        }
        
    }
}
