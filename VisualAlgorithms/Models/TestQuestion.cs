using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Тип вопроса")] 
        public QuestionType QuestionType { get; set; }

        public int Answer { get; set; }

        public int TestId { get; set; }
    }

    public enum QuestionType
    {
        [Display(Name = "Свободный ответ")] FreeAnswer,
        [Display(Name = "С выбором ответа")] SelectAnswer
    }
}