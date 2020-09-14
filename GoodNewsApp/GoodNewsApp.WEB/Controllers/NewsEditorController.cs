using AutoMapper;
using GoodNewsApp.BusinessLogic;
using GoodNewsApp.BusinessLogic.Services;
using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using GoodNewsApp.DataAccess.Repository;
using GoodNewsApp.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.WEB.Controllers
{
    [Authorize]
    public class NewsEditorController : Controller
    {
        //private readonly GoodNewsAppContext _context;
        //private readonly UnitOfWork _unitOfWork;
        //private readonly IMapper _iMapper;
        private readonly NewsService _newsService;




        //GoodNewsAppContext context, UnitOfWork unitOfWork, IMapper iMapper, 
        public NewsEditorController(NewsService newsService)
        {
            //_context = context;
            //_unitOfWork = unitOfWork;
            //_iMapper= iMapper;
            _newsService = newsService;

    }


        // GET: News
        public async Task<IActionResult> List()
        {
            return View(await _newsService.GetAllAsync(HomeController.cancellationToken));
        }



        
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



        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateViewModel newsFromView) //[Bind("Title,Content,SourseURL")] 
        {
            if (ModelState.IsValid)
            {
                NewsDTO newsDTO = new NewsDTO()
                {
                    Id = Guid.NewGuid(),
                    Title = newsFromView.Title,
                    Body = newsFromView.Body,
                    SourseURL = newsFromView.SourseURL,
                    CreatedOnDate = DateTime.Now,
                    EditedOnDate = DateTime.Now,
                };

                await _newsService.AddAsync(newsDTO);
               /* await _unitOfWork.NewsRepository.AddAsync(_news);
                await _unitOfWork.SaveChangeAsync();
               */
                return RedirectToAction("Index","Home"); //List

            }
            return View(newsFromView);
        }

        // GET: News/Edit/5
        
        
        public async Task<IActionResult> Edit(Guid id) //Guid? id //cancellationToken
        {
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

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsDetailsViewModel newsFromView)//
        {
            NewsDTO newsDTOToChange =  await _newsService.GetByIdAsync(newsFromView.Id, HomeController.cancellationToken);
            if (newsFromView.Id != newsDTOToChange.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    newsDTOToChange.Title = newsFromView.Title;
                    newsDTOToChange.Body = newsFromView.Body;
                    newsDTOToChange.SourseURL = newsFromView.SourseURL;
                    newsDTOToChange.EditedOnDate = DateTime.Now;

                     await _newsService.Update(newsDTOToChange);

                    
                }
                catch
                {
                    return NotFound();

                }
                return RedirectToAction("Index", "Home");
            }
            return View(newsFromView);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(Guid id) //Guid? 
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsDTO newsDTOToDelete = await _newsService.GetByIdAsync(id, HomeController.cancellationToken);

            if (newsDTOToDelete == null)
            {
                return NotFound();
            }

            return View(newsDTOToDelete);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
           
            await _newsService.Delete(id);
            

            return RedirectToAction("Index", "Home");
        }

        
    }
}
