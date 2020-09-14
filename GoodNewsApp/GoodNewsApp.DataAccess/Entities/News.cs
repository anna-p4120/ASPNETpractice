using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Entities
{
    public class News : IEntity
    {
        //[ScaffoldColumn(false)]
        public Guid Id { get; set; }

        //[Required(ErrorMessage = "Поле должно быть заполнено")]
        //[Display(Name = "Заголовок")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Поле должно быть заполнено")]
       //[Display(Name = "Содержание")]
        //[DataType(DataType.MultilineText)]
        public string Body { get; set; }

       // [Required(ErrorMessage = "Поле должно быть заполнено")]
       // [Display(Name = "Источник")]
        public string SourseURL { get; set; }

        //[Display(Name = "Дата создания")]
        public DateTime CreatedOnDate { get; set; }

       // [Display(Name = "Дата редактирования")]
        public DateTime EditedOnDate { get; set; }

        public virtual IEnumerable<Comment> Comment { get; set; }




    }
}
