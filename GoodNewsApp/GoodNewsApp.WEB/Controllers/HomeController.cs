using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoodNewsApp.WEB.Models;
using GoodNewsApp.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using GoodNewsApp.DataAccess.Repository;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System.Security.Claims;
using Hangfire;
using GoodNewsApp.BusinessLogic.Helpers;
using GoodNewsApp.BusinessLogic.Services.NewsServices;
using GoodNewsApp.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GoodNewsApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;

        //TODO
        public static CancellationToken cancellationToken = new CancellationTokenSource().Token;

               
        public HomeController(ILogger<HomeController> logger, INewsService newsService)
        {
            _logger = logger;       
            _newsService = newsService;
        }



        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllAsync(cancellationToken);
            return View(newsList.OrderByDescending(n => n.CreatedOnDate));
        }


        [Authorize(AuthenticationSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Details(Guid id)
        {
            //var n = HttpContext.User.Identity.Name;

            if (id == null)
            {
                return NotFound();
            }

            NewsDTO newsDTObyId = await _newsService.GetByIdAsync(id, HomeController.cancellationToken);
            if (newsDTObyId == null)
            {
                return NotFound();
            }

            NewsDetailsViewModel _newsDetailsViewModel = new NewsDetailsViewModel()
            {
                Id = newsDTObyId.Id,
                Title = newsDTObyId.Title,
                Body = newsDTObyId.Body,
                SourseURL = newsDTObyId.SourseURL,
                CreatedOnDate = newsDTObyId.CreatedOnDate,
                EditedOnDate = newsDTObyId.EditedOnDate,
            };

            return View(_newsDetailsViewModel);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
