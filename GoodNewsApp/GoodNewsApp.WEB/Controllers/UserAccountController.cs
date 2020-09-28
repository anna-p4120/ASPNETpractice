using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsApp.BusinessLogic.Helpers;
using GoodNewsApp.BusinessLogic.Interfaces;
using GoodNewsApp.BusinessLogic.Services.UsersServices;
using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using GoodNewsApp.DataAccess.Repository;
using GoodNewsApp.WEB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsApp.WEB.Controllers
{
    public class UserAccountController : Controller
    {
        
        private readonly IUsersService _usersService;

        public UserAccountController(IUsersService usersService)
        {
            
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (Request.HttpContext.Request.Path.HasValue)
                {
                    ViewBag.ReturnURL = returnUrl;
                }
                else
                {
                    //!!! const
                    ViewBag.returnURL = "/Home/Index";
                }
                return View();
            }
            return Redirect(returnUrl ?? "/Home/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                UserDTO userFromDB = await _usersService.GetUserByEmailAsync(registerViewModel.Email);

                if (userFromDB == null)
                {
                   
                    string passwordHash, passwordSalt;

                    PasswordManager.CreatePasswordHash(registerViewModel.Password, out passwordHash, out passwordSalt);

                    UserDTO newUserDTO = new UserDTO()
                    {
                        Id = Guid.NewGuid(),
                        Name = registerViewModel.Name ?? registerViewModel.Email,
                        Email = registerViewModel.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    };

                    CreatedUserRole newUserParams = await _usersService.CreateUserRoleAsync(newUserDTO, DefaultRolesList.User.ToString()); //, RolesList.Admin.ToString()"Admin", 

                    await AuthenticateAsync(newUserParams.Email, newUserParams.UserRolesID[0]);

                }
                else
                {
                    ModelState.AddModelError("", "Такой пользователь существует");

                    //return Content("Такой пользователь существует");

                    return View();

                }

                return Redirect(registerViewModel.ReturnURL ?? "/Home/Index");

            }

            return View();
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if(!User.Identity.IsAuthenticated)
            {
                if (Request.HttpContext.Request.Path.HasValue)
                {
                    ViewBag.ReturnURL = returnUrl;

                }
                else
                {
                    //TODO: move to const
                    ViewBag.returnURL = "/Home/Index";
                } 
                return View();

            }

            //напрямую?
            return Redirect(returnUrl ?? "/Home/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {

                UserDTO userFromDB = await _usersService.GetUserByEmailAsync(loginViewModel.Email);

                if (userFromDB != null)
                {
                    if (PasswordManager.VerifyPasswordHash(loginViewModel.Password, userFromDB.PasswordHash, userFromDB.PasswordSalt))
                    {
                      
                       List<Guid> userFromDBRoleId = _usersService.GetRoleIdByUserId(userFromDB.Id);
                        Guid roleID = userFromDBRoleId[0];

                        await AuthenticateAsync(userFromDB.Name, roleID);

                    }
                        
                }

                else

                {
                    ModelState.AddModelError("", "Укажите правильный логин и(или) пароль");
                    
                    return View();

                }

                return Redirect(loginViewModel.ReturnURL ?? "/Home/Index");

            }

            return View();
        }


        //TODO userName replace by email
        private async Task AuthenticateAsync (string userEmail, Guid roleId)
        {
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleId.ToString())
            };
            ClaimsIdentity Id = new ClaimsIdentity
                (
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
                );

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(Id));

            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
      
    }
}
