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
        public IEnumerable<TestQuestion> TestQuestions { get; set; }
        public IEnumerable<UserTest> UserTests { get; set; }
    }
}
