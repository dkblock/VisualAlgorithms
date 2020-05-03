using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VisualAlgorithms.Models
{
    public class ApplicationUser : IdentityUser
    {
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

        public Group Group { get; set; }
        public IEnumerable<UserAnswer> TestAnswers { get; set; }
        public IEnumerable<UserTest> UserTests { get; set; }
    }
}
