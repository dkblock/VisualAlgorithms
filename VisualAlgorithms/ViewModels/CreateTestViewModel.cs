using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class CreateTestViewModel
    {
        [Display(Name = "Название теста")]
        public string TestName { get; set; }

        [Display(Name = "Структура данных")]
        public int AlgorithmId { get; set; }

        public IEnumerable<Algorithm> Algorithms { get; set; } 
    }
}
