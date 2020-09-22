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

namespace GoodNewsApp.WebAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public static CancellationToken cancellationToken = new CancellationTokenSource().Token;

        public UsersController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {          
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;


        }


        //!string.IsNullOrEmpty(registerModel.Email)
        //        && !string.IsNullOrEmpty(registerModel.Password)
        //        && string.Compare(registerModel.ConfirmPassword, registerModel.Password) == 0


        //TODO compare string object

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel registerModel) //
        {
            if (ModelState.IsValid) 
            {

                User userFromDB = await _unitOfWork.UserRepository.FindBy(u => u.Email == registerModel.Email).FirstOrDefaultAsync();
                if (userFromDB == null)
                {
                    try
                    {
                        string passwordHash, passwordSalt;

                        PasswordManager.CreatePasswordHash(registerModel.Password, out passwordHash, out passwordSalt);

                        User newUser = new User()
                        {
                            Id = Guid.NewGuid(),
                            Name = registerModel.Name ?? registerModel.Email,
                            Email = registerModel.Email,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt
                        };

                        Guid defaultRoleId = (await _unitOfWork.RoleRepository.FindBy(u => u.Name == "User").FirstOrDefaultAsync()).Id;

                        UserRole newUserRole = new UserRole()
                        {
                            Id = Guid.NewGuid(),
                            UserId = newUser.Id,
                            RoleId = defaultRoleId,
                            RegistrationDate = DateTime.Now,

                        };

                        
                        await _unitOfWork.UserRepository.AddAsync(newUser);

                        await _unitOfWork.UserRoleRepository.AddAsync(newUserRole);
                        
                        await _unitOfWork.SaveChangeAsync();

                        return Ok(); //newUser


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
                User userFromDB = await _unitOfWork.UserRepository.
                    FindBy(u => string.Compare(u.Email, loginModel.Email) == 0)
                  .FirstOrDefaultAsync();

                if (userFromDB != null)
                {

                    if (PasswordManager.VerifyPasswordHash(loginModel.Password, userFromDB.PasswordHash, userFromDB.PasswordSalt))
                    {
                        // ! if more than one...?
                        Guid userFromDBRoleId = 
                         (await _unitOfWork.UserRoleRepository.
                         FindBy(u => u.UserId == userFromDB.Id)
                         .FirstOrDefaultAsync())
                         .RoleId;
          
                         string tokenString = PasswordManager.CreateToken(userFromDB.Name, userFromDBRoleId, _appSettings.Secret);


                        // return basic user info and authentication token
                        return Ok(tokenString);

                        /*return Ok(new
                        {
                            Id = userFromDB.Id,
                            Name = userFromDB.Name,
                            Email = userFromDB.Email,
                            UserRole = userFromDB.UserRole,
                            Token = tokenString
                        });*/

                    }

                }
                return BadRequest(new { message = "User don't exist"});

            }
            return BadRequest(new { message = "Username or password is incorrect" });
 
        }
        
    }
}
