using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VisualAlgorithms.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Группа")]
        public string Group { get; set; }

        public IEnumerable<UserAnswer> TestAnswers { get; set; }
        public IEnumerable<UserTest> UserTests { get; set; }
    }
}
