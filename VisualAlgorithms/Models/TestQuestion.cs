using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите вопрос!")]
        [Display(Name = "Вопрос")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Выберите тип вопроса!")]
        [Display(Name = "Тип вопроса")]
        public TestQuestionType TestQuestionType { get; set; }

        public int CorrectAnswerId { get; set; }

        public string Image { get; set; }

        public bool IsLastQuestion { get; set; }

        public int TestId { get; set; }

        public Test Test { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
    }

    public enum TestQuestionType
    {
        SelectAnswer = 0,
        FreeAnswer = 1
    }
}