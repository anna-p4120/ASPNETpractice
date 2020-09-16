﻿using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoodNewsApp.DataAccess.Entities;
using AutoMapper;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsApp.BusinessLogic.Services.NewsServices
{
    public class NewsService
    {
        private readonly GoodNewsAppContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NewsService(GoodNewsAppContext context, UnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context; ////!!!!!
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(NewsDTO newsDTO) //[Bind("Title,Content,SourseURL")] 
        {
            // News newsCreated = new News();
            News newsCreated = _mapper.Map<News>(newsDTO);

            await _unitOfWork.NewsRepository.AddAsync(newsCreated);
            await _unitOfWork.SaveChangeAsync();

        }


        public async Task<NewsDTO> GetByIdAsync(Guid id, CancellationToken token)
        {

            News newsById = await _unitOfWork.NewsRepository.GetByIdAsync(id, token);
            NewsDTO newsDTObyId = _mapper.Map<NewsDTO>(newsById);
            return newsDTObyId;


        }

        public async Task<List<NewsDTO>> GetAllAsync(CancellationToken token)
        {

            List<News> newsAll = await _unitOfWork.NewsRepository.GetAllAsync(token);
            List<NewsDTO> newsDTOAll = _mapper.Map<List<NewsDTO>>(newsAll);
            return newsDTOAll;


        }

        public async Task Update(NewsDTO newsDTO)
        {

            News newsToUpdate = _mapper.Map<News>(newsDTO);
            try
            {

                _unitOfWork.NewsRepository.Update(newsToUpdate);

                await _unitOfWork.SaveChangeAsync();


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(newsToUpdate.Id))
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }


        }


        public async Task Delete(Guid id) //[Bind("Title,Content,SourseURL")] 
        {


            try
            {
                await _unitOfWork.NewsRepository.Delete(id);
                await _unitOfWork.SaveChangeAsync();

            }
            catch
            {
                throw;
            }



        }

        private bool NewsExists(Guid id)
        {
            return _context.News.Any(e => e.Id == id);
        }

    }
}