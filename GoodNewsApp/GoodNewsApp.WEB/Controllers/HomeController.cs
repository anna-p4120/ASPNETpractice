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
using HtmlAgilityPack;
using GoodNewsApp.BusinessLogic.Services.NewsServices;

namespace GoodNewsApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsService _newsService;

        //TODO
        public static CancellationToken cancellationToken = new CancellationTokenSource().Token;

        //TODO
        private readonly DbInitializer _dbInitializer;
       
        public HomeController(ILogger<HomeController> logger, DbInitializer dbInitializer, NewsService newsService)
        {
            _logger = logger;
            _dbInitializer = dbInitializer;
            _newsService = newsService;
        }



        public async Task<IActionResult> Index()
        {
            
            // TODO: move to????
            await _dbInitializer.InitializeWithUsersAndRolesAsync();

            
            var newsList = await _newsService.GetAllAsync(cancellationToken);
            return View(newsList.OrderByDescending(n => n.CreatedOnDate));


        }

        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            var n = HttpContext.User.Identity.Name;

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
