using AutoMapper;
using GoodNewsApp.BusinessLogic.Services.NewsServices;
using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.BusinessLogic.Mapping
{
    public class NewsProfile: Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDTO>().ReverseMap();
            //CreateMap<NewsDTO, News>();
        }
    }
}
