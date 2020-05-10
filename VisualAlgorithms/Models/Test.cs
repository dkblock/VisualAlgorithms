using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название!")]
        [StringLength(50, ErrorMessage = "Название теста должно содержать от 3 до 50 символов!", MinimumLength = 3)]
        [Display(Name = "Название теста")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Выберите алгоритм!")]
        [Display(Name = "Алгоритм")]
        public int AlgorithmId { get; set; }

        public Algorithm Algorithm { get; set; }
        public List<TestQuestion> TestQuestions { get; set; }
        public List<UserTest> UserTests { get; set; }
    }
}
