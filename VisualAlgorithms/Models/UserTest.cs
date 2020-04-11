using System;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class UserTest
    {
        [Display(Name = "Результат (%)")]
        public double Result { get; set; }

        [Display(Name = "Дата прохождения")]
        public DateTime PassionTime { get; set; }

        public string UserId { get; set; }

        public int TestId { get; set; }

        public ApplicationUser User { get; set; }
        public Test Test { get; set; }
    }
}
