using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите Email!")]
        [EmailAddress(ErrorMessage = "Неверный формат Email!")]
        [StringLength(256, ErrorMessage = "Длина не должна превышать 256 символов!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов!")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию!")]
        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов!")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Выберите группу!")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите группу!")]
        [Display(Name = "Группа")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен состоять минимум из 6 символов!", MinimumLength = 6)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }

        public List<Group> Groups { get; set; }
    }
}
