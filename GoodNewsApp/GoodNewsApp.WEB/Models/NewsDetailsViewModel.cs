using GoodNewsApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodNewsApp.WEB.Models
{
    public class NewsDetailsViewModel
    {
        //[ScaffoldColumn(false)] // [HiddenInput(DisplayValue=false)]
        [HiddenInput]
        public Guid Id { get; set; }

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
        
        [ScaffoldColumn(false)]
        [Display(Name = "Дата создания")]
        public DateTime CreatedOnDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Дата последнего редактирования")]
        public DateTime EditedOnDate { get; set; }

        /*[ScaffoldColumn(false)]
        [Display(Name = "Комментарии")]
        public virtual IEnumerable<Comment> Comment { get; set; }
        */


    }
}