using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Введите Email!")]
        [EmailAddress(ErrorMessage = "Неверный формат Email!")]
        [StringLength(256, ErrorMessage = "Длина не должна превышать 256 символов!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен состоять минимум из 6 символов!", MinimumLength = 6)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }

        public string Code { get; set; }
    }
}
