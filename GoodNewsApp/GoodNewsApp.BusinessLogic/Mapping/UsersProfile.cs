using AutoMapper;
using GoodNewsApp.BusinessLogic.Services.NewsServices;
using GoodNewsApp.BusinessLogic.Services.UsersServices;
using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.BusinessLogic.Mapping
{
    public class UsersProfile: Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            

        }
    }
}
