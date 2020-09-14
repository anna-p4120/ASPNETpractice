using System.ComponentModel.DataAnnotations;

namespace GoodNewsApp.WEB.Models
{
    public class RegisterViewModel {
               
        
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [MaxLength(10, ErrorMessage = "Не более 10 символов")] // ? email length
        [Display(Name = "Имя")]
        public string Name { get; set; }

       
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        
        [Compare("Password",ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        public string ReturnURL { get; set; }


    }
}
