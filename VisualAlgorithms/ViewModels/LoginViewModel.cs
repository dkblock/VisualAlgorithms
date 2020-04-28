using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Email!")]
        [Display(Name = "Email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
