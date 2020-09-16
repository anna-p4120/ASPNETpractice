using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace GoodNewsApp.BusinessLogic.Services.NewsServices
{
    public class NewsDTO
    {

        public Guid Id { get; set; }



        public string Title { get; set; }



        public string Body { get; set; }



        public string SourseURL { get; set; }


        public DateTime CreatedOnDate { get; set; }


        public DateTime EditedOnDate { get; set; }


        public IEnumerable<Comment> Comment { get; set; }

    }
}
