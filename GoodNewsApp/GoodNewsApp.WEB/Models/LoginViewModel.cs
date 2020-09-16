using System.ComponentModel.DataAnnotations;


namespace GoodNewsApp.WEB.Models
{
    public class LoginViewModel
    {


        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        // by Filter?
        public string ReturnURL { get; set; }


    }
}
