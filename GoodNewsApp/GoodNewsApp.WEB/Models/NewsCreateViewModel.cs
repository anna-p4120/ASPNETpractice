using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodNewsApp.WEB.Models
{
    public class NewsCreateViewModel
    {
               
        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Содержание")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Body { get; set; }

        [Display(Name = "Источник")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string SourseURL { get; set; }

        
    }
}