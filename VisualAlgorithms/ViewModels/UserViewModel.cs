using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов!")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию!")]
        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов!")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public int GroupId { get; set; }
        public string Role { get; set; }
        public List<Group> Groups { get; set; }
        public List<string> Roles { get; set; }
    }
}
