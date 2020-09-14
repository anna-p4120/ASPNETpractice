using AutoMapper;
using GoodNewsApp.BusinessLogic;
using GoodNewsApp.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.WEB.Mapping
{
    public class NewsViewProfile : Profile
    {
        public NewsViewProfile()
        {
            
            CreateMap<NewsDTO, NewsListViewModel>().ReverseMap();
        }
    }
}
