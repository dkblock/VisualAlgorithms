using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualAlgorithms.Models
{
    public class UserTest
    {
        [Display(Name = "Результат (%)")]
        public double Result { get; set; }

        [Display(Name = "Правильных ответов")]
        public int CorrectAnswers { get; set; }

        [Display(Name = "Всего вопросов")]
        public int TotalQuestions { get; set; }

        [Display(Name = "Дата прохождения")]
        public DateTime PassingTime { get; set; }

        public string UserId { get; set; }

        public int TestId { get; set; }

        public ApplicationUser User { get; set; }
        public Test Test { get; set; }
    }
}
