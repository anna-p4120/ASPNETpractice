using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsApp.GoodNewsAppDomainModel.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GoodNewsApp.Models;


namespace GoodNewsApp.Models
{
    public class NewsViewModel
    {
       
        public Guid Id { get; set; }

        // [Required (ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }


        // [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Содержание")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        // [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Источник")]
        public string SourseURL { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedOnDate { get; set; }

        

        


    }
}
