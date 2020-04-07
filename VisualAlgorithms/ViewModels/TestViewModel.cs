using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.ViewModels
{
    public class TestViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")] 
        public string TestName { get; set; }

        [Display(Name = "Структура данных")] 
        public string AlgorithmName { get; set; }

        [Display(Name = "Количество вопросов")]
        public int QuestionsCount { get; set; }
    }
}