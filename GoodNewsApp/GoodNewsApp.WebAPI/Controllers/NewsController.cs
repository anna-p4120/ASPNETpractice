using GoodNewsApp.BusinessLogic.Interfaces;
using GoodNewsApp.BusinessLogic.Services.NewsServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;

        private readonly INewsService _newsService;

        public static CancellationToken cancellationToken = new CancellationTokenSource().Token;

        public NewsController(ILogger<NewsController> logger, INewsService newsService)
        {
         _logger = logger;
         _newsService = newsService;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var newsList = await _newsService.GetAllAsync(cancellationToken);
                return Ok(newsList);

            }
            catch
            {
                return StatusCode(500);
            }
        }

    }
}
