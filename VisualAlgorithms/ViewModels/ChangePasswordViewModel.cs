using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен состоять минимум из 6 символов!", MinimumLength = 6)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
    }
}
