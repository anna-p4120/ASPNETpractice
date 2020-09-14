using GoodNewsApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodNewsApp.WEB.Models
{
    public class NewsListViewModel
    {

        //[ScaffoldColumn(false)]
        //[HiddenInput]
        //[HiddenInput(DisplayValue = false)]
        // public Guid Id { get; set; }


        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        
        [Display(Name = "Содержание")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        
        [Display(Name = "Источник")]
        public string SourseURL { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedOnDate { get; set; }

        [Display(Name = "Дата последнего редактирования")]
        public DateTime EditedOnDate { get; set; }

        [Display(Name = "Комментарии")]
        public virtual IEnumerable<Comment> Comment { get; set; }


    }
}