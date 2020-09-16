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

namespace GoodNewsApp.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
   [Route("api/users")]
    class UsersController : ControllerBase
    {
        //private readonly GoodNewsAppContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public static CancellationToken cancellationToken = new CancellationTokenSource().Token;
        public UsersController(UnitOfWork unitOfWork, IOptions<AppSettings> appSettings) //GoodNewsAppContext context, 
        {
           // _context = context;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var newsList = await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);
                return Ok(newsList);

            }
            catch
            {
                return StatusCode(500);
            }
        }*/


        [AllowAnonymous]
      [HttpPost]
        
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel registerModel) //
        {
            if (!string.IsNullOrEmpty(registerModel.Email)
                && !string.IsNullOrEmpty(registerModel.Password)
                && string.Compare(registerModel.ConfirmPassword, registerModel.Password) == 0) //compare string object
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
                            PasswordSalt = passwordSalt//CreatePasswordHash
                        };

                        Guid defaultRoleId = (await _unitOfWork.RoleRepository.FindBy(u => u.Name == "User").FirstOrDefaultAsync()).Id;

                        UserRole newUserRole = new UserRole()
                        {
                            Id = Guid.NewGuid(),
                            UserId = newUser.Id,
                            RoleId = defaultRoleId,
                            RegistrationDate = DateTime.Now,

                        };

                        // CREATE
                        // create user
                        await _unitOfWork.UserRepository.AddAsync(newUser);

                        await _unitOfWork.UserRoleRepository.AddAsync(newUserRole);

                        //await Authenticate(newUser.Name, defaultRoleId);

                        await _unitOfWork.SaveChangeAsync();


                        return Ok();//newUser



                    }
                    catch (Exception ex)
                    {
                        // return error message if there was an exception
                        return BadRequest(new { message = ex.Message });
                    }

                }
                return BadRequest(new { message = "Такой пользователь существует" });

            }
            return BadRequest(new { message = "Username or password is incorrect" });


        }


       [AllowAnonymous]
       [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            if (!string.IsNullOrEmpty(loginModel.Email) && !string.IsNullOrEmpty(loginModel.Password))
            {
                User userFromDB = await _unitOfWork.UserRepository.
                    FindBy(u => u.Email == loginModel.Email)// && u.PasswordHash == loginModel.Password
                    .FirstOrDefaultAsync();

                if (userFromDB != null)
                {

                    if (!PasswordManager.VerifyPasswordHash(loginModel.Password, userFromDB.PasswordHash, userFromDB.PasswordSalt))
                    {
                        // ! if more than one...?
                        Guid userFromDBRoleId = (await _unitOfWork.UserRoleRepository.
                         FindBy(u => u.UserId == userFromDB.Id).FirstOrDefaultAsync())
                         .RoleId;

                         string tokenString = CreateToken(userFromDB.Name, userFromDBRoleId);


                        // return basic user info and authentication token
                        return Ok(new
                        {
                            Id = userFromDB.Id,
                            Name = userFromDB.Name,
                            Email = userFromDB.Email,
                            UserRole = userFromDB.UserRole,
                            Token = tokenString
                        });
                     
                    }

                }
                
            }
            return BadRequest(new { message = "Username or password is incorrect" });
 
        }

        private string CreateToken(string userName, Guid roleId)
        {
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleId.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            
             var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                 /*Subject = new ClaimsIdentity(new Claim[]
                 {
                     new Claim(ClaimTypes.Name, user.Id.ToString())
                 }),*/
        
               Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
            
            //return "1";

        }
    }
}
