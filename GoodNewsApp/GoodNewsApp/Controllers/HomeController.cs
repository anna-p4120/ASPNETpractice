using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoodNewsApp.GoodNewsAppDomainModel.Context;
using Microsoft.EntityFrameworkCore;
using GoodNewsApp.Models;


namespace GoodNewsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GoodNewsAppContext _context;
        private readonly NewsViewModel newsViewModel = new NewsViewModel();


        public HomeController(ILogger<HomeController> logger, GoodNewsAppContext context)
        {
            _logger = logger;
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            var newsList = await _context.News.ToListAsync();

            var newsViewModelList = newsList.Select(newsList => new NewsViewModel()
            {
                Id = newsList.Id,
                Title = newsList.Title,
                Body = newsList.Body,
                SourseURL = newsList.SourseURL,
                CreatedOnDate = newsList.CreatedOnDate

            }).ToList();
               
  
            return View(newsViewModelList);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsOfId = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsOfId == null)
            {
                return NotFound();
            }

            return View(newsOfId);
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
