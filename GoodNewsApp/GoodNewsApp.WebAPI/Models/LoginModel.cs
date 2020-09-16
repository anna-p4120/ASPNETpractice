using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodNewsApp.WebAPI.Models
{
    class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Не указан пароль")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Пароль")]
        public string Password { get; set; }

       
    }
}
