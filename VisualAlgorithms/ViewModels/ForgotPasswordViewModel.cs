using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Введите Email!")]
        [EmailAddress(ErrorMessage = "Неверный формат Email!")]
        [StringLength(256, ErrorMessage = "Длина не должна превышать 256 символов!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
