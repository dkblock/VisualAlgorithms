using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.ViewModels
{
    public class TestViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Тест")] 
        public string TestName { get; set; }

        [Display(Name = "Структура данных")] 
        public string AlgorithmName { get; set; }
    }
}