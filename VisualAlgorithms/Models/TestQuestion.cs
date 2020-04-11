using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }

        [Display(Name = "Вопрос")]
        public string Question { get; set; }

        [Display(Name = "Ответ")]
        public int Answer { get; set; }

        public bool IsLastQuestion { get; set; }

        public int TestId { get; set; }

        public Test Test { get; set; }
        public IEnumerable<UserAnswer> UserAnswers { get; set; }
    }

    public enum QuestionType
    {
        [Display(Name = "Свободный ответ")] FreeAnswer,
        [Display(Name = "С выбором ответа")] SelectAnswer
    }
}