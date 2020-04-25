using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Display(Name = "Название теста")]
        public string Name { get; set; }

        public int AlgorithmId { get; set; }

        public Algorithm Algorithm { get; set; }
        public List<TestQuestion> TestQuestions { get; set; }
        public List<UserTest> UserTests { get; set; }
    }
}
