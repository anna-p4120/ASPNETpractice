using AutoMapper;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Services.NewsServices
{
    public class NewsRangeLoader
    {


        // private readonly NewsService _newsService;NewsService newsService,
        //;NewsService newsService, 
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NewsRangeLoader(UnitOfWork unitOfWork, IMapper mapper)
        {
            //_newsService = newsService;
            // _newsDTO = newsDTO;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<bool> UpdateRangeByUrlAsync(List<NewsDTO> newsDTOToStore, CancellationToken token)
        {

            List<News> newsAll = await _unitOfWork.NewsRepository.GetAllAsync(token);


            //List<NewsDTO> newsDTOFromDB = await _newsService.GetAllAsync(token);


            //replace by update
            foreach (var n in newsDTOToStore)
            {
                News newsCreated = _mapper.Map<News>(n);

                if (newsAll.Any(e => e.SourseURL == n.SourseURL))
                {
                    //News newsToDelete = _mapper.Map<News>(n);

                    Guid newsToDeleteId = _unitOfWork.NewsRepository
                        .FindBy(e => e.SourseURL == n.SourseURL)
                        .FirstOrDefault()
                        .Id;

                    try
                    {

                        await _unitOfWork.NewsRepository.Delete(newsToDeleteId);


                    }
                    catch (DbUpdateConcurrencyException)
                    {

                        throw;

                    }

                }

                //else
                // {


                await _unitOfWork.NewsRepository.AddAsync(newsCreated);
                // }
            }
            //await _newsService.Update(n);
            await _unitOfWork.SaveChangeAsync();

            return true;
        }
    }
}



