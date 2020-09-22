using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly GoodNewsAppContext _context;
        private readonly IUnitOfWork _unitOfWork;


        public UserAccountController(GoodNewsAppContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
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

                User userFromDB = await _unitOfWork.UserRepository.FindBy(u => u.Email == registerViewModel.Email).FirstOrDefaultAsync();
                if (userFromDB == null)
                {
                    string passwordHash, passwordSalt;

                    PasswordManager.CreatePasswordHash(registerViewModel.Password, out passwordHash, out passwordSalt);
                    User newUser = new User()
                    {
                        Id = Guid.NewGuid(),
                        Name = registerViewModel.Name ?? registerViewModel.Email,
                        Email = registerViewModel.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    };

                    await _unitOfWork.UserRepository.AddAsync(newUser);

                    //TODO: default role as const
                    Guid defaultRoleId = (await _unitOfWork.RoleRepository.FindBy(u => u.Name == "User").FirstOrDefaultAsync()).Id;

                    UserRole newUserRole = new UserRole()
                    {
                        Id = Guid.NewGuid(),
                        UserId = newUser.Id,
                        RoleId = defaultRoleId,
                        RegistrationDate = DateTime.Now,

                    };

                    await _unitOfWork.UserRoleRepository.AddAsync(newUserRole);

                    await AuthenticateAsync(newUser.Name, defaultRoleId);

                    await _unitOfWork.SaveChangeAsync();

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

                User userFromDB = await _unitOfWork.UserRepository.
                    FindBy(u => u.Email == loginViewModel.Email)// && u.PasswordHash == loginViewModel.Password
                    .FirstOrDefaultAsync();

                if (userFromDB != null)
                {
                    if (PasswordManager.VerifyPasswordHash(loginViewModel.Password, userFromDB.PasswordHash, userFromDB.PasswordSalt))
                    {
                        // ! if more than one...?
                        Guid userFromDBRoleId = (await _unitOfWork.UserRoleRepository.
                        FindBy(u => u.UserId == userFromDB.Id).FirstOrDefaultAsync())
                        .RoleId;

                        await AuthenticateAsync(userFromDB.Name, userFromDBRoleId);

                        await _unitOfWork.SaveChangeAsync();

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
        private async Task AuthenticateAsync (string userName, Guid roleId)
        {
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
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
