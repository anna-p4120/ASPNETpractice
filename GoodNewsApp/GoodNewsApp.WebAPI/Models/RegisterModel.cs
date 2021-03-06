﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodNewsApp.WebAPI.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [MaxLength(10, ErrorMessage = "Не более 10 символов")] // ? email length
        [Display(Name = "Имя")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        

    }
}
