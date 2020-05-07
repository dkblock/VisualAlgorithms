using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название!")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Доступна для регистрации")]
        public bool IsAvailableForRegister { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
